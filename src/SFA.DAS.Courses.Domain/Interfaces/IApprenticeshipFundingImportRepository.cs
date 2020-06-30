using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IApprenticeshipFundingImportRepository
    {
        Task InsertMany(IEnumerable<ApprenticeshipFundingImport> apprenticeshipFundingImports);
        void DeleteAll();
    }
}