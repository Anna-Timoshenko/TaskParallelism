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
            //string jObjectString = File.ReadAllText("tasks2.json");
            string jObjectString = File.ReadAllText(args[0]);
            JObject obj = JObject.Parse(jObjectString);
            List<Task> tasks = new List<Task>();

            _ = RunTasks(obj, tasks);

            Console.ReadKey();
        }

        static async Task RunTasks(JObject obj, List<Task> tasks, string nameParent = null, bool check = false)
        {
            JToken tokenObj;

            foreach (var token in obj)
            {
                tokenObj = obj[token.Key];
                var prop = (JProperty)tokenObj.Parent;

                if (tokenObj.Type.ToString() == "Integer")
                {
                    int duration = (int)tokenObj;

                    if (nameParent == "Sequence")
                    {
                        await Task.WhenAll(tasks.ToArray());
                        await Task.Run(() => TaskSuccessivelyAsync(prop.Name, duration));
                        continue;
                    }

                    if (check)
                    {
                        tasks.Add(Task.Run(() => TaskParallel(prop.Name, duration)));
                        continue;
                    }

                    check = true;
                    _ = Task.Run(() => TaskParallel(prop.Name, duration));
                }
                else if (tokenObj.Type.ToString() == "Object")
                {
                    _ = RunTasks((JObject)tokenObj, tasks, prop.Name, check);
                }
            }
        }

        static Task TaskParallel(string name, int sec)
        {
            return Task.Run(async () =>
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
