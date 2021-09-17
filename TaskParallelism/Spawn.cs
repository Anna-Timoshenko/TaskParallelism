using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskParallelism
{
    public class Spawn : INodeContainer
    {
        public Spawn()
        {
            Tasks = new List<INode>();
        }

        public ICollection<INode> Tasks { get; set; }

        public async Task RunAsync()
        {
            var taskList = new List<Task>();

            foreach (var task in Tasks)
            {
                taskList.Add(task.RunAsync());
            }

            await Task.WhenAll(taskList);
        }
    }
}