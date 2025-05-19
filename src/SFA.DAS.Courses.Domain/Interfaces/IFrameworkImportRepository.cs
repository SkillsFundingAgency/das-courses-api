using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworkImportRepository
    {
        Task InsertMany(IEnumerable<FrameworkImport> frameworks);
        Task DeleteAll();
        Task<IEnumerable<FrameworkImport>> GetAll();
    }
}
