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

        public List<INode> Tasks { get; set; }

        public async Task Run()
        {
            List<Task> taskList = new List<Task>();

            foreach (var item in Tasks)
            {
                taskList.Add(item.Run());
            }

            await Task.WhenAll(taskList);
        }
    }
}
