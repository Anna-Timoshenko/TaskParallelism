using System.Collections.Generic;

namespace TaskParallelism
{
    public interface INodeContainer : INode
    {
        public ICollection<INode> Tasks { get; set; }
    }
}