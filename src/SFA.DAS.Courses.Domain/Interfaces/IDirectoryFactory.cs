using Lucene.Net.Store;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IDirectoryFactory
    {
        BaseDirectory GetDirectory();
    }
}
