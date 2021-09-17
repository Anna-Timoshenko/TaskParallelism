using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TaskParallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            string jObjectString = File.ReadAllText(args[0]);
            JObject json = JObject.Parse(jObjectString);

            Run(json);

            Console.ReadKey();
        }

        static void Parse(JObject json, INodeContainer node)
        {
            JToken jToken;

            foreach (var token in json)
            {
                jToken = json[token.Key];

                if (token.Key == "Sequence")
                {
                    Sequence sequence = new Sequence();

                    node.Tasks.Add(sequence);
                    Parse((JObject)jToken, sequence);
                }
                else if (token.Key == "Spawn")
                {
                    Spawn spawn = new Spawn();

                    node.Tasks.Add(spawn);
                    Parse((JObject)jToken, spawn);
                }
                else
                {
                    int duration = (int)jToken;
                    var prop = (JProperty)jToken.Parent;
                    SimpleTask task = new SimpleTask(prop.Name, duration);

                    node.Tasks.Add(task);
                }
            }
        }

        static void Run(JObject json)
        {
            INodeContainer root = new Spawn();

            Parse(json, root);

            root.RunAsync();
        }
    }
}
