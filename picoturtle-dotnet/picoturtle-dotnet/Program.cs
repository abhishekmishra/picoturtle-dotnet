using System;
using System.Diagnostics;
using picoturtle;

namespace picoturtledotnet
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Turtle t = new picoturtle.Turtle();
            t.Init(250, 250);
            Console.WriteLine("Created Turtle with name -> " + t.name);
            t.pendown();
            t.pencolour(128, 128, 0);
            for (int i = 0; i < 4; i++)
            {
                t.forward(100);
                t.right(90);
            }
            t.stop();
            Process.Start(t.BrowserURL());
        }
    }
}
