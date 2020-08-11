using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorSubjectAreaTier2Repository
    {
        void DeleteAll();
        Task InsertMany(IEnumerable<SectorSubjectAreaTier2> sectorSubjectAreaTier2Items);
    }
}