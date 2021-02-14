using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp2.Client.PHP
{
    public class BlazorWriter : TextWriter
    {
        private RenderTreeBuilder builder { get; set; } = null;
        private int index { get; set; } = 1;

        public void StartRender(RenderTreeBuilder builder)
        {
            this.builder = builder;
        }

        public void StopRender()
        {
            this.builder = null;
            index = 0;
        }

        public override void Write(char value)
        {
            if (builder == null)
                throw new Exception("Builder tree must be set!");

            builder.AddContent(index++, value);
        }

        public override void Write(string value)
        {
            if (builder == null)
                throw new Exception("Builder tree must be set!");

            builder.AddContent(index++, value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
