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
            string jObjectString = File.ReadAllText("tasks3.json");
            //string jObjectString = File.ReadAllText(args[0]);

            JObject obj = JObject.Parse(jObjectString);
            List<Task> tasks = new List<Task>();

            Run(obj, tasks);

            Console.ReadKey();
        }

        static async Task Run(JObject obj, List<Task> tasks, string nameParent = null, bool check = false)
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
                        check = true;
                        while (!CheckCompletion(tasks))
                        {
                            await Task.WhenAll(tasks.ToArray());
                        }
                        
                        Task task = RunTasks(prop.Name, duration);

                        tasks.Add(task);
                        await task;

                        continue;
                    }

                    if (check)
                    {
                        check = false;
                        tasks.Add(Task.Run(() => RunTasks(prop.Name, duration)));
                        continue;
                    }

                    Task.Run(() => RunTasks(prop.Name, duration));
                }
                else
                {
                    Run((JObject)tokenObj, tasks, prop.Name, check);
                }
            }
        }

        static async Task RunTasks(string name, int duration)
        {
            Console.WriteLine($"{name} start");

            for (int i = duration; i >= 0; --i)
            {
                Console.WriteLine($"{name} {i}");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine($"{name} end");
        }

        static bool CheckCompletion(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Status != TaskStatus.RanToCompletion)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
