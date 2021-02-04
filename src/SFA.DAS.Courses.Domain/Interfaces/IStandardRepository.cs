using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardRepository
    {
        Task<int> Count(StandardFilter filter);
        void DeleteAll();
        Task InsertMany(IEnumerable<Standard> standards);
        Task<Standard> Get(int larsCode);
        Task<Standard> Get(string standardUId);
        Task<IEnumerable<Standard>> GetStandards(IList<Guid> routeIds, IList<int> levels, StandardFilter filter);
        Task<IEnumerable<Standard>> GetStandards(string iFateReferenceNumber);
    }
}
