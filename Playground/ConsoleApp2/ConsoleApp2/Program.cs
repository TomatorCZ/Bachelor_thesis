using Pchp.Core;
using System;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Context.AddScriptReference(typeof(foo.PhpClass).Assembly);
            
            Context ctx = Context.CreateConsole("index2.php");
            
            var script = Context.TryGetDeclaredScript("index2.php");
            script.Evaluate(ctx, ctx.Globals, null);
            
            
            Console.WriteLine("Hello World!");
        }
    }
}
