using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

ScriptState<object>? scriptState = null;
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();                    // read

    if (input == "exit") break;

    scriptState = scriptState == null ?                // eval
        await CSharpScript.RunAsync(input) :                    
        await scriptState.ContinueWithAsync(input);

    Console.WriteLine(scriptState.ReturnValue);        // print
}                                                      // loop!
