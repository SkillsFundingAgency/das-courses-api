using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFundingImportRepository
    {
        Task InsertMany(IEnumerable<FundingImport> fundingImports);
        Task DeleteAll();
        Task<IEnumerable<FundingImport>> GetAll();
    }
}
