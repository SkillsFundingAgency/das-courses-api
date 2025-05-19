using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsStandardImportRepository
    {
        Task InsertMany(IEnumerable<LarsStandardImport> larsStandardImports);
        Task DeleteAll();
        Task<IEnumerable<LarsStandardImport>> GetAll();
    }
}
