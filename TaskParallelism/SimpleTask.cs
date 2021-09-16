using System;
using System.Threading.Tasks;

namespace TaskParallelism
{
    public class SimpleTask : INode
    {
        public SimpleTask(string name, int duration)
        {
            Name = name;
            Duration = duration;
        }

        public string Name { get; set; }
        public int Duration { get; set; }

        public async Task Run()
        {
            Console.WriteLine($"{Name} start");

            for (int i = Duration; i >= 0; --i)
            {
                Console.WriteLine($"{Name} {i}");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine($"{Name} end");
        }
    }
}
