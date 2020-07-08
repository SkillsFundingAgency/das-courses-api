using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworkFundingImportRepository
    {
        Task InsertMany(IEnumerable<FrameworkFundingImport> frameworkFundingImports);
        void DeleteAll();
        Task<IEnumerable<FrameworkFundingImport>> GetAll();
    }
}