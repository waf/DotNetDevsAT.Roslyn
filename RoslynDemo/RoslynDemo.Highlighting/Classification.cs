// adapted from https://github.com/dotnet/roslyn-sdk/blob/main/samples/CSharp/ConsoleClassifier/Program.cs

using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

public class Classification
{
    public ClassifiedSpan ClassifiedSpan { get; private set; }
    public string Text { get; private set; }

    public Classification(string classification, TextSpan span, SourceText text) :
        this(classification, span, text.GetSubText(span).ToString())
    {
    }

    public Classification(string classification, TextSpan span, string text) :
        this(new ClassifiedSpan(classification, span), text)
    {
    }

    public Classification(ClassifiedSpan classifiedSpan, string text)
    {
        ClassifiedSpan = classifiedSpan;
        Text = text;
    }

    public string Type => ClassifiedSpan.ClassificationType;

    public TextSpan TextSpan => ClassifiedSpan.TextSpan;
}
