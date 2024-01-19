using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorSubjectAreaTier1ImportRepository
    {
        Task<IEnumerable<SectorSubjectAreaTier1Import>> GetAll();
        void DeleteAll();
        Task InsertMany(IEnumerable<SectorSubjectAreaTier1Import> sectorSubjectAreaTier1Imports);
    }
}
