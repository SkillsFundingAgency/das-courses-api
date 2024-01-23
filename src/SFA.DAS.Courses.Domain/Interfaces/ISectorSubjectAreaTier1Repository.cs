using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorSubjectAreaTier1Repository
    {
        void DeleteAll();
        Task InsertMany(IEnumerable<SectorSubjectAreaTier1> sectorSubjectAreaTier1Items);
        Task<List<SectorSubjectAreaTier1>> GetAll(CancellationToken cancellationToken);
    }
}
