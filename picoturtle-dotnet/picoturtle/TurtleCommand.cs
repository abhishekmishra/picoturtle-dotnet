using System;
using System.Collections.Generic;

namespace picoturtle
{
    public class TurtleCommand
    {
        private string commandName { get; }
        private List<string> valueList { get; set; }
        private Dictionary<string, string> valueObj { get; set; }
        private bool valueIsObj { get; }

        public TurtleCommand(string cmd, bool isObj)
        {
            commandName = cmd;
            valueIsObj = isObj;
            if(valueIsObj)
            {
                valueObj = new Dictionary<string, string>();
            }
            else
            {
                valueList = new List<string>();
            }
        }
    }
}
