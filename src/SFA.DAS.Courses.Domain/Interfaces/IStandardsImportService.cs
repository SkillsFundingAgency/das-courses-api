using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsImportService
    {
        Task<(bool Success, List<string> ValidationMessages)> ImportDataIntoStaging();
        Task LoadDataFromStaging(DateTime timeStarted);
    }
}
