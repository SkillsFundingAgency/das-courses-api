using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworksImportService
    {
        Task ImportDataIntoStaging();
    }
}
