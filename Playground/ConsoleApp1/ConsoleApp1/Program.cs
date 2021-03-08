using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Foo1("Hello", 1, "World", new int[] { 1,2});
        }

        public static void Foo1(params object[] args)
        {
            Foo2(args);
        }

        public static void Foo2(params object[] args)
        {
            ;
        }
    }
}
