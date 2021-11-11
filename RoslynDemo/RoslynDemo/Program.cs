using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var code = File.ReadAllText("HelloWorldInput.cs");
SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

SyntaxNode root = tree.GetRoot();

LiteralExpressionSyntax? oldMessage = root
    .DescendantNodes()
    .OfType<LiteralExpressionSyntax>()
    .Single();

LiteralExpressionSyntax? newMessage = SyntaxFactory.LiteralExpression(
    SyntaxKind.StringLiteralExpression,
    SyntaxFactory.Literal("Hello Roslyn!")
);

SyntaxNode newProgram = root.ReplaceNode(oldMessage, newMessage);

File.WriteAllText("HelloRoslynOutput.cs", newProgram.ToFullString());
