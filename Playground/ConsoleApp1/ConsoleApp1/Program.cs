using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClassLibrary2;
using Pchp.Core;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new PHPClass();

            Assembly phpassembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "ClassLibrary1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Context.AddScriptReference(phpassembly);

            var ctx = Context.CreateEmpty();

            using MemoryStream buffer = new MemoryStream();
            using StreamWriter writer = new StreamWriter(buffer, System.Text.Encoding.UTF8);
            ctx.Output = writer;

            var script = Context.TryGetDeclaredScript("index.php");

            script.Evaluate(ctx, ctx.Globals, null);

            writer.Flush();
            buffer.Position = 0;
            using StreamReader reader = new StreamReader(buffer, System.Text.Encoding.UTF8);
            Console.WriteLine(reader.ReadToEnd());

        }

       
    }
}
