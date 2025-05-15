using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardImportRepository
    {
        Task InsertMany(IEnumerable<StandardImport> standardImports);
        Task DeleteAll();
        Task<IEnumerable<StandardImport>> GetAll();
    }
}
