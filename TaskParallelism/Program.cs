using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TaskParallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = "tasks.json";

            Dictionary<string, int> spawTasks = new Dictionary<string, int>();
            Dictionary<string, int> sequenceTasks = new Dictionary<string, int>();

            //JObject obj = JObject.Parse(File.ReadAllText(fileName));
            JObject obj = JObject.Parse(File.ReadAllText(args[0]));

            var spawList = obj["Spaw"].ToList<JToken>();
            var sequenceList = obj["Spaw"]["Sequence"].ToList<JToken>();

            foreach (var spaw in spawList)
            {
                JProperty property = spaw.ToObject<JProperty>();

                if (property.Name == "Sequence")
                {
                    foreach (var seq in sequenceList)
                    {
                        JProperty propertySeq = seq.ToObject<JProperty>();
                        sequenceTasks[propertySeq.Name] = (int)propertySeq.Value;
                    }

                    continue;
                }

                spawTasks[property.Name] = (int)property.Value;
            }

            Task.Factory.StartNew(() => {

                foreach (var item in spawTasks)
                {
                     Task.Run(async () => {
                        Console.WriteLine($"{item.Key} start");

                        for (int i = item.Value; i >= 0; --i)
                        {
                            Console.WriteLine($"{item.Key} {i}");
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }

                        Console.WriteLine($"{item.Key} end");
                    });
                }
            });

            Task.Factory.StartNew(async () => {

                foreach (var item in sequenceTasks)
                {
                    await Task.Run(async () => {
                        Console.WriteLine($"{item.Key} start");

                        for (int i = item.Value; i >= 0; --i)
                        {
                            Console.WriteLine($"{item.Key} {i}");
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }

                        Console.WriteLine($"{item.Key} end");
                    });
                }
            });

            Console.ReadKey();
        }
    }
}