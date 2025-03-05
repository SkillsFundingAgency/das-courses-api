using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorSubjectAreaTier2ImportRepository
    {
        Task<IEnumerable<SectorSubjectAreaTier2Import>> GetAll();
        Task DeleteAll();
        Task InsertMany(IEnumerable<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports);
    }
}
