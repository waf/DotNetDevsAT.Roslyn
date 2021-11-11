// with minor modifications from https://github.com/dotnet/roslyn-sdk/blob/main/samples/CSharp/ConsoleClassifier/Program.cs

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

// build up an in-memory solution/project structure for demo purposes.
AdhocWorkspace workspace = new AdhocWorkspace();
Solution solution = workspace.CurrentSolution;
Project project = solution.AddProject("projectName", "assemblyName", LanguageNames.CSharp);
Document document = project.AddDocument("name.cs",
@"class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
    }
}");

// perform the syntax classification -- this tells us that e.g. from indexes 0 to index 5 there's a keyword
SourceText text = await document.GetTextAsync();
IEnumerable<ClassifiedSpan> classifiedSpans = await Classifier
    .GetClassifiedSpansAsync(document, TextSpan.FromBounds(0, text.Length));
var classifications = classifiedSpans
    .Select(classifiedSpan => new Classification(classifiedSpan, text.GetSubText(classifiedSpan.TextSpan).ToString()));

// minor detail - where we don't have any classification, fill it in with a default value; this will display in white text.
classifications = FillGaps(text, classifications);

// print the code in colors to the console
Console.BackgroundColor = ConsoleColor.Black;
foreach (Classification classification in classifications)
{
    Console.ForegroundColor = classification.Type switch
    {
        ClassificationTypeNames.Keyword => ConsoleColor.Magenta,
        ClassificationTypeNames.ClassName => ConsoleColor.Cyan,
        ClassificationTypeNames.StringLiteral => ConsoleColor.Yellow,
        _ => ConsoleColor.White,
    };
    Console.Write(classification.Text);
}

Console.ResetColor();
Console.WriteLine();


static IEnumerable<Classification> FillGaps(SourceText text, IEnumerable<Classification> ranges)
{
    const string? WhitespaceClassification = null;
    int current = 0;
    Classification? previous = null;

    foreach (Classification range in ranges)
    {
        int start = range.TextSpan.Start;
        if (start > current)
        {
            yield return new Classification(WhitespaceClassification, TextSpan.FromBounds(current, start), text);
        }

        if (previous == null || range.TextSpan != previous.TextSpan)
        {
            yield return range;
        }

        previous = range;
        current = range.TextSpan.End;
    }

    if (current < text.Length)
    {
        yield return new Classification(WhitespaceClassification, TextSpan.FromBounds(current, text.Length), text);
    }
}
