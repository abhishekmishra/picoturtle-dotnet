using System.Collections.Generic;

namespace picoturtle
{
    public class Colour
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public int a { get; set; }
        public string hex { get; set; }

        public Colour()
        {
        }
    }

    public class Location
    {
        public double x { get; set; }
        public double y { get; set; }

        public Location()
        {
        }
    }


    public class TurtleState
    {
        public Colour colour;
        public Location location;
        public double angle;
        public bool pen;
        public double pen_width;
        public string id;
        public string name;
        public int last;
        public string font_str;

        public TurtleState()
        {
        }
    }
}
