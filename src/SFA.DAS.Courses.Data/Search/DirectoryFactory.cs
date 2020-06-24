using Lucene.Net.Store;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Search
{
    public class DirectoryFactory : IDirectoryFactory
    {
        private BaseDirectory _baseDirectory;

        public BaseDirectory GetDirectory()
        {
            return _baseDirectory ?? (_baseDirectory = new RAMDirectory());
        }
    }
}
