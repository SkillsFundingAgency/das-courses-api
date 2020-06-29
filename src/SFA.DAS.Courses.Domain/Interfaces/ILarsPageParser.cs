using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsPageParser
    {
        Task<string> GetCurrentLarsDataDownloadFilePath();
    }
}