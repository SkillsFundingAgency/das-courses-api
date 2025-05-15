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
        Task DeleteAll();
        Task<int> InsertMany(IEnumerable<Standard> standards);
        Task<Standard> GetLatestActiveStandard(int larsCode);
        Task<Standard> GetLatestActiveStandard(string iFateReferenceNumber);
        Task<Standard> Get(string standardUId);
        Task<IEnumerable<Standard>> GetStandards();
        Task<IEnumerable<Standard>> GetStandards(IList<int> routeIds, IList<int> levels, StandardFilter filter, bool includeAllProperties);
        Task<IEnumerable<Standard>> GetStandards(string iFateReferenceNumber);
    }
}
