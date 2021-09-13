using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TaskParallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            //string jObjectString = File.ReadAllText("tasks.json");
            string jObjectString = File.ReadAllText(args[0]);
            JObject obj = JObject.Parse(jObjectString);

            RunTasks(obj);

            Console.ReadKey();
        }

        static async void RunTasks(JObject obj)
        {
            Dictionary<string, int> spaws = new Dictionary<string, int>();
            Dictionary<string, int> sequences = new Dictionary<string, int>();

            ParseTreeJObject(obj, spaws, sequences);

            foreach (var spaw in spaws)
            {
                TaskParallel(spaw.Key, spaw.Value);
            }

            foreach (var sequence in sequences)
            {
                await Task.Run(async () => await TaskSuccessivelyAsync(sequence.Key, sequence.Value));
            }
        }

        static void ParseTreeJObject(JObject obj, Dictionary<string, int> spaw, Dictionary<string, int> seq, string nameParent = null)
        {
            JToken i;

            foreach (var k in obj)
            {
                i = obj[k.Key];
                var prop = (JProperty)i.Parent;

                if (i.Type.ToString() == "Integer")
                {
                    if (nameParent == "Sequence")
                    {
                        seq[prop.Name] = (int)i;
                        continue;
                    }

                    spaw[prop.Name] = (int)i;
                }
                else if (i.Type.ToString() == "Object")
                {
                    ParseTreeJObject((JObject)i, spaw, seq, prop.Name);
                }
            }
        }

        static void TaskParallel(string name, int sec)
        {
            Task.Run(async () =>
            {
                Console.WriteLine($"{name} start");

                for (int i = sec; i >= 0; --i)
                {
                    Console.WriteLine($"{name} {i}");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                Console.WriteLine($"{name} end");
            });
        }

        static async Task TaskSuccessivelyAsync(string name, int sec)
        {
            await Task.Run(async () =>
            {
                Console.WriteLine($"{name} start");

                for (int i = sec; i >= 0; --i)
                {
                    Console.WriteLine($"{name} {i}");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                Console.WriteLine($"{name} end");
            });
        }
    }
}
