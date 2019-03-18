using System;
using System.Collections.Generic;

namespace picoturtle
{
    public class TurtleCommand
    {
        public string cmd { get; }
        public List<Object> args { get; set; }

        public TurtleCommand(string cmdName)
        {
            cmd = cmdName;
            args = new List<object>();
        }
    }
}
