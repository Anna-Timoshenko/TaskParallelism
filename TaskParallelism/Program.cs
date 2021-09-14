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
            List<Task> tasks = new List<Task>();

            RunTasks(obj, tasks);

            Console.ReadKey();
        }

        static async void RunTasks(JObject obj, List<Task> tasks, string nameParent = null)
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
                        Task.WaitAll(tasks.ToArray());
                        await Task.Run(async () => await TaskSuccessivelyAsync(prop.Name, (int)i));
                        continue;
                    }

                    tasks.Add(Task.Run(() => TaskParallel(prop.Name, (int)i)));
                }
                else if (i.Type.ToString() == "Object")
                {
                    RunTasks((JObject)i, tasks, prop.Name);
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
