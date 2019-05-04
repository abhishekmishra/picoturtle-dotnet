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
            Console.WriteLine("Created Turtle with name -> " + t.name);
            t.canvas_size(250, 250);
            t.penup();
            t.setpos(125, 125);
            t.pendown();
            t.pencolour(128, 128, 0);
            for (int i = 0; i < 4; i++)
            {
                t.forward(100);
                t.right(90);
            }
            t.export_img("cs-dotnet-picoturtle-test.png");
            t.stop();
            Process.Start(t.BrowserURL());
        }
    }
}
