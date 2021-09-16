using System.Threading.Tasks;

namespace TaskParallelism
{
    public interface INode
    {
        public Task RunAsync();
    }
}