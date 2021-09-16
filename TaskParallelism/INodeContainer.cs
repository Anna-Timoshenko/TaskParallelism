using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskParallelism
{
    public interface INodeContainer : INode
    {
        List<INode> Tasks { get; set; }
    }
}
