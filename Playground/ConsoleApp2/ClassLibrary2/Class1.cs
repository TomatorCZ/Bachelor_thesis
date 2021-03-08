using Pchp.Core;
using System;
using System.Collections.Generic;

namespace ClassLibrary2
{
    [PhpType]
    public class Class1
    {
        protected string name;

        public Class1()
        {
            this.name = ""; 
        }

        public void __construct(string name)
        { 
            this.name = name;
        }

        public string GetName() => this.name;
    }

    [PhpType]
    public class Class2
    {
        protected List<string> ar;

        public Class2()
        {
            this.ar = new List<string>();
        }

        public Class2(string name)
        {
            this.ar = new List<string>();
            ar.Add(name);
        }

        public PhpAlias this[string name]
        {
            get { return PhpValue.FromClass(ar).AsPhpAlias(); }
        }

        public void write()
        {
            foreach (var item in ar)
            {
                Console.WriteLine(item);
            }
        }
    }
}
