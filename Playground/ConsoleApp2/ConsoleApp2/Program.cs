using Pchp.Core;
using System;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            //Context ctx = Context.CreateEmpty();

            var a = new foo.PhpClass();
            string n = a.GetName();
            
            
            Console.WriteLine("Hello World!");
        }
    }
}
