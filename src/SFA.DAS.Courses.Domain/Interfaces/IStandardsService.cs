using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsService
    {
        Task<IEnumerable<Standard>> GetStandardsList(string keyword, IList<Guid> routeIds, IList<int> levels, OrderBy orderBy, bool filterAvailableToStart = true);
        Task<int> Count();
        Task<Standard> GetStandard(int standardId);
    }
}
