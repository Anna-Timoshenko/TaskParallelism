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

        public List<INode> Tasks { get; set; }

        public async Task Run()
        {
            foreach (var item in Tasks)
            {
                await item.Run();
            }
        }
    }
}
