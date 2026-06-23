using System;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsDataImportService
    {
        Task<(bool Success, string FileName)> ImportDataIntoStaging();
        Task LoadDataFromStaging(DateTime importAuditStartTime, string filePath);
    }
}
