using System;

namespace danl.adventofcode2020
{
    class Program
    {
        static Program()
        {
            
        }

        static void Main(string[] args)
        {
            GetPuzzleToRun(args);

            var method = typeof(CrabCups23.CrabCups23).GetMethod("Run", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            method.Invoke(null, new object[] { 2 });
        }

        public static void GetPuzzleToRun(string[] args)
        {
        }
    }
}
