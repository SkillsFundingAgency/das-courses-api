using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorImportRepository
    {
        Task InsertMany(IEnumerable<SectorImport> sector);
        Task<IEnumerable<SectorImport>> GetAll();
        void DeleteAll();
    }
}