using System;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworksImportService
    {
        Task<(bool Success, string LatestFile)> ImportDataIntoStaging();
        Task LoadDataFromStaging(DateTime importStartTime, string latestFile);
    }
}
