using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;

namespace PhpBlazor
{
    class BlazorWriter : TextWriter
    {
        private Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder _builder;
        private int _counter;

        #region Create
        private BlazorWriter(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) : base()
        {
            _builder = builder;
            _counter = 0;
        }

        public static BlazorWriter CreateConsole()
        {
            return new BlazorWriter(null);
        }

        public static BlazorWriter CreateTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            return new BlazorWriter(builder);
        }
        #endregion

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string value)
        {
            if (_builder == null)
                Console.Write(value);
            else
                _builder.AddMarkupContent(_counter++, value);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _builder = null;
        }
    }
}
