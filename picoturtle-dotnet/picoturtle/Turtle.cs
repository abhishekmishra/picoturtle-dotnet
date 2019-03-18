using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace picoturtle
{
    public class Turtle
    {

        public string name { get; set; }
        private string turtleUrl;
        private bool bulk;
        private int bulkLimit;
        private List<TurtleCommand> commands;
        private HttpClient client;

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
            client = new HttpClient();
            //if (this.name == null)
            //{
            //    this.Init();
            //}
        }

        private TurtleState TurtleRequest(String cmd, List<KeyValuePair<string, Object>> args = null, bool is_obj = false)
        {
            if (this.bulk == true && (cmd != "create"))
            {
                TurtleCommand command = new TurtleCommand(cmd);
                if (args != null && args.Count > 0)
                {
                    if (is_obj)
                    {
                        var arg_obj = new Dictionary<string, Object>();
                        foreach (var kvpair in args)
                        {
                            arg_obj[kvpair.Key] = kvpair.Value;
                        }
                        command.args.Add(arg_obj);
                    }
                    else
                    {
                        foreach (var kvpair in args)
                        {
                            command.args.Add(kvpair.Value);
                        }
                    }
                }
                this.commands.Add(command);
                if ((this.commands.Count >= this.bulkLimit) || (cmd == "stop") || (cmd == "state"))
                {
                    //Debug.WriteLine("Draining commands " + this.commands.Count);
                    string json = JsonConvert.SerializeObject(this.commands);
                    //Debug.WriteLine(json);
                    string url = this.turtleUrl + "/turtle/" +
                                             this.name + "/commands";
                    //Debug.WriteLine(url);
                    try
                    {
                        Task<HttpResponseMessage> resTask = client.PostAsync(url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        resTask.Wait();
                        Task<string> responseTextTask = resTask.Result.Content.ReadAsStringAsync();
                        responseTextTask.Wait();
                        string responseText = responseTextTask.Result;

                        //Debug.WriteLine(responseText);
                        var t = JsonConvert.DeserializeObject<TurtleState>(responseText);
                        return t;
                    }
                    catch (HttpRequestException e)
                    {
                        Debug.WriteLine("\nException Caught!");
                        Debug.WriteLine("Message :{0} ", e.Message);
                    }

                    this.commands = new List<TurtleCommand>();
                }
            }
            else
            {
                var request_url = "/turtle/";
                if (this.name != null)
                {
                    request_url += this.name;
                    request_url += "/";
                }
                request_url += cmd;
                if (args != null)
                {
                    request_url += "?";
                    int i = 0;
                    foreach (var arg in args)
                    {
                        if (i > 0)
                        {
                            request_url += "&";
                        }
                        request_url += arg.Key;
                        request_url += "=";
                        request_url += arg.Value.ToString();
                        i++;
                    }
                }
                string json = JsonConvert.SerializeObject(this.commands);
                try
                {
                    //Debug.WriteLine(request_url);
                    Task<string> resTask = client.GetStringAsync(this.turtleUrl + request_url);
                    resTask.Wait();
                    string responseText = resTask.Result;

                    //Debug.WriteLine(responseText);
                    var t = JsonConvert.DeserializeObject<TurtleState>(responseText);
                    return t;
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine("\nException Caught!");
                    Debug.WriteLine("Message :{0} ", e.Message);
                }
            }
            return null;
        }

        public string BrowserURL()
        {
            return turtleUrl + "/index.html?details=0&list=0&name=" + name;
        }

        public TurtleState Init(double x=250, double y=250)
        {
            commands = new List<TurtleCommand>();
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("x", x));
            args.Add(new KeyValuePair<string, Object>("y", y));
            TurtleState t = TurtleRequest("create", args);
            this.name = t.name;
            return t;
        }

        public TurtleState penup()
        {
            return TurtleRequest("penup");
        }

        public TurtleState pendown()
        {
            return TurtleRequest("pendown");
        }

        public TurtleState penwidth(double w)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("w", w));

            return TurtleRequest("penwidth", args);
        }

        public TurtleState stop()
        {
            return TurtleRequest("stop");
        }

        public TurtleState state()
        {
            return TurtleRequest("state");
        }

        public TurtleState home()
        {
            return TurtleRequest("home");
        }

        public TurtleState clear()
        {
            return TurtleRequest("clear");
        }

        public TurtleState forward(double d)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("d", d));

            return TurtleRequest("forward", args);
        }

        public TurtleState back(double d)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("d", d));

            return TurtleRequest("back", args);
        }

        public TurtleState setpos(double x, double y)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("x", x));
            args.Add(new KeyValuePair<string, Object>("y", y));

            return TurtleRequest("goto", args);
        }

        public TurtleState setx(double x)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("x", x));

            return TurtleRequest("setx", args);
        }

        public TurtleState sety(double y)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("y", y));

            return TurtleRequest("sety", args);
        }

        public TurtleState left(double a)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("a", a));

            return TurtleRequest("left", args);
        }

        public TurtleState right(double a)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("a", a));

            return TurtleRequest("right", args);
        }

        public TurtleState heading(double a)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("a", a));

            return TurtleRequest("heading", args);
        }

        public TurtleState font(string f)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("f", f));

            return TurtleRequest("font", args);
        }

        public TurtleState filltext(string text)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("t", text));

            return TurtleRequest("filltext", args);
        }

        public TurtleState stroketext(string text)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("t", text));

            return TurtleRequest("stroketext", args);
        }

        public TurtleState pencolour(int r, int g, int b)
        {
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("r", r));
            args.Add(new KeyValuePair<string, Object>("g", g));
            args.Add(new KeyValuePair<string, Object>("b", b));

            return TurtleRequest("pencolour", args, true);
        }
    }


}
