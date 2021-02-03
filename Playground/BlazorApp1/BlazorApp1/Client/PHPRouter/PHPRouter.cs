using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Pchp.Core;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlazorApp1.Client.PHPRouter
{
    [Route("/{Script}")]
    public class PHPRouter : ComponentBase
    {
        [Parameter] public string Script { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private Context ctx { get; set; } = Context.CreateEmpty();
        private MemoryStream _mem { get; set; } = new MemoryStream();
        private StreamWriter _stream { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            new PHPClass();
            Context.AddScriptReference(Assembly.Load("ClassLibrary1"));


            var a = Context.TryGetDeclaredScript("index.php");
            

            a.Evaluate(ctx,ctx.Globals,null);
            _stream.Flush();
            builder.AddContent(0, _mem.ToArray().ToB64String());
        }

        protected override void OnParametersSet()
        {
            _stream = new StreamWriter(_mem, Encoding.ASCII, 1024,true);
            ctx.Output = _stream;
            base.OnParametersSet();
        }

        Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}
