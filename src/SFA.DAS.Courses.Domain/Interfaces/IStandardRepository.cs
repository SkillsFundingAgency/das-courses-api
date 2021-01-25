using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardRepository
    {
        Task<IEnumerable<Standard>> GetAll(StandardFilter filter);
        Task<int> Count(StandardFilter filter);
        void DeleteAll();
        Task InsertMany(IEnumerable<Standard> standards);
        Task<Standard> Get(int id);
        Task<IEnumerable<Standard>> GetFilteredStandards(IList<Guid> routeIds, IList<int> levels, StandardFilter filter);
    }
}
