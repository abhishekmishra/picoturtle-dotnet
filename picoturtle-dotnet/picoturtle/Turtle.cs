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
            if (this.name == null)
            {
                this.Init();
            }
        }

        private TurtleState CreateRequest(String cmd, List<KeyValuePair<string, Object>> args, bool is_obj = false)
        {
            if (this.bulk == true && (cmd != "create"))
            {
                TurtleCommand command = new TurtleCommand(cmd);
                if (args.Count > 0)
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
                    //print(this.commands)
                    //drain the commands
                    string json = JsonConvert.SerializeObject(this.commands);
                    try
                    {
                        Task<HttpResponseMessage> resTask = client.PostAsync(this.turtleUrl + "/turtle/" +
                                             this.name + "/commands", new StringContent(json));
                        resTask.Wait();
                        Task<string> responseTextTask = resTask.Result.Content.ReadAsStringAsync();
                        responseTextTask.Wait();
                        string responseText = responseTextTask.Result;

                        Debug.WriteLine(responseText);
                    }
                    catch (HttpRequestException e)
                    {
                        Debug.WriteLine("\nException Caught!");
                        Debug.WriteLine("Message :{0} ", e.Message);
                    }

                    //    req = urllib.request.Request(this.turtle_url + "/turtle/" +
                    //                             this.name + "/commands")
                    //req.add_header("Content-Type",
                    //               "application/json; charset=utf-8")
                    //jsondata = json.dumps(this.commands)
                    //jsondataasbytes = jsondata.encode("utf-8")  # needs to be bytes
                    //req.add_header("Content-Length", len(jsondataasbytes))
                    //res = urllib.request.urlopen(req, jsondataasbytes)
                    //# print("Draining " + str(len(this.commands)) + " commands for " +
                    //#       this.name)
                    //this.commands = []
                    //t = json.loads(res.read().decode("utf-8"))
                    //return t
                }
            }
            else
            {
                var request_url = "/turtle/";
                if (this.name != null) {
                    request_url += this.name;
                    request_url += "/";
                }
                request_url += cmd;
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
                }
                string json = JsonConvert.SerializeObject(this.commands);
                try
                {
                    Task<string> resTask = client.GetStringAsync(this.turtleUrl + request_url);
                    resTask.Wait();
                    string responseText = resTask.Result;

                    Debug.WriteLine(responseText);
                    var t = JsonConvert.DeserializeObject<TurtleState>(responseText);
                    Debug.WriteLine(t.location.x);
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

        TurtleState Init()
        {
            commands = new List<TurtleCommand>();
            var args = new List<KeyValuePair<string, Object>>();
            args.Add(new KeyValuePair<string, Object>("x", 250));
            args.Add(new KeyValuePair<string, Object>("y", 250));
            TurtleState t = CreateRequest("create", args);
            this.name = t.name;
            return t;
        }
    }
}
