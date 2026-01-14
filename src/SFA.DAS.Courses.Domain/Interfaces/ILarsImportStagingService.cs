using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsImportStagingService
    {
        Task Import(string filePath);
    }
}
