using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardRepository
    {
        Task<IEnumerable<Standard>> GetAll(bool filterAvailableToStart = true);
        Task<int> Count();
        void DeleteAll();
        Task InsertMany(IEnumerable<Standard> standards);
        Task<Standard> Get(int id);
        Task<IEnumerable<Standard>> GetFilteredStandards(IList<Guid> routeIds, IList<int> levels);
    }
}
