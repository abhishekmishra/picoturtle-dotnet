using System;
using picoturtle;

namespace picoturtledotnet
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Turtle t = new picoturtle.Turtle();
            Console.WriteLine("Created Turtle with name -> " + t.name);
        }
    }
}
