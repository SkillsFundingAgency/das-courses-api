using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsDataDownloadService
    {
        Task<Stream> GetFileStream(string downloadPath);
    }
}