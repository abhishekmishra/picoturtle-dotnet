using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace picoturtle
{
    public class Turtle
    {
        private string name;
        private string turtleUrl;
        private bool bulk;
        private int bulkLimit;
        private List<TurtleCommand> commands;

        public Turtle(string name = null,
                 string host = "127.0.0.1",
                 int port = 3000,
                 bool bulk = true,
                 int bulkLimit = 100)
        {
            this.name = name;
            this.turtleUrl = "http://" + host + ":" + port;
            this.bulk = bulk;
            this.bulkLimit = bulkLimit;
            this.name = name;
            this.commands = new List<TurtleCommand>();
            if (this.name == null) {
                //self.turtle_init()

            }
            this.createRequest(null);
        }

        private void createRequest(String cmd)
        {
            // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    Task<string> resTask = client.GetStringAsync("http://www.abhishekmishra.in/");
                    resTask.Wait();
                    string response = resTask.Result;

                    Debug.WriteLine(response);
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine("\nException Caught!");
                    Debug.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}
