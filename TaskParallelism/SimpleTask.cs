using System;
using System.Threading.Tasks;

namespace TaskParallelism
{
    public class SimpleTask : INode
    {
        private readonly string _name;
        private readonly int _duration;

        public SimpleTask(string name, int duration)
        {
            _name = name;
            _duration = duration;
        }

        public async Task RunAsync()
        {
            Console.WriteLine($"{_name} start");

            for (int i = _duration; i >= 0; --i)
            {
                Console.WriteLine($"{_name} {i}");

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine($"{_name} end");
        }
    }
}