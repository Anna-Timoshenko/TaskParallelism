using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskParallelism
{
    public class Sequence : INodeContainer
    {
        public Sequence()
        {
            Tasks = new List<INode>();
        }

        public ICollection<INode> Tasks { get; set; }

        public async Task RunAsync()
        {
            foreach (var task in Tasks)
            {
                await task.RunAsync();
            }
        }
    }
}