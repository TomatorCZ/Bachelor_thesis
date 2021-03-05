using Pchp.Core;
using System;

namespace ClassLibrary2
{
    [PhpType]
    public class Class1
    {
        protected string name;

        public Class1(string name)
        {
            this.name = name; 
        }

        public string GetName() => this.name;
    }
}
