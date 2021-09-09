using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TaskParallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "tasks.json";

            Dictionary<string, int> spawTasks = new Dictionary<string, int>();
            Dictionary<string, int> sequenceTasks = new Dictionary<string, int>();

            JObject obj = JObject.Parse(File.ReadAllText(fileName));

            var spawList = obj["Spaw"].ToList<JToken>();
            var sequenceList = obj["Spaw"]["Sequence"].ToList<JToken>();

            foreach (var item in spawList)
            {
                JProperty property = item.ToObject<JProperty>();

                if (property.Name == "Sequence")
                {
                    continue;
                }

                spawTasks[property.Name] = (int)property.Value;
            }

            foreach (var item in sequenceList)
            {
                JProperty property = item.ToObject<JProperty>();

                sequenceTasks[property.Name] = (int)property.Value;
            }

            Task.Factory.StartNew(() => {

                foreach (var item in spawTasks)
                {
                     Task.Factory.StartNew(() => {
                        Console.WriteLine($"{item.Key} start");

                        for (int i = item.Value; i >= 0; --i)
                        {
                            Console.WriteLine($"{item.Key} {i}");
                            //await Task.Delay(TimeSpan.FromSeconds(1));
                            Thread.Sleep(1000);
                        }

                        Console.WriteLine($"{item.Key} end");
                    });
                }

            });

            Task.Factory.StartNew(async () => {

                foreach (var item in sequenceTasks)
                {
                    await Task.Factory.StartNew(() => {
                        Console.WriteLine($"{item.Key} start");

                        for (int i = item.Value; i >= 0; --i)
                        {
                            Console.WriteLine($"{item.Key} {i}");
                            //await Task.Delay(TimeSpan.FromSeconds(1));
                            Thread.Sleep(1000);
                        }

                        Console.WriteLine($"{item.Key} end");
                    });
                }

            });

            Console.ReadKey();
        }
    }
}