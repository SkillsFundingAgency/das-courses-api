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
        Task<Standard> GetLatestActiveStandard(string larsCode);
        Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber);
        Task<List<Standard>> GetActiveStandardsByIfateReferenceNumbers(List<string> ifateReferenceNumbers);
        Task<Standard> Get(string standardUId);
        Task<IEnumerable<Standard>> GetStandards();
        Task<IEnumerable<Standard>> GetStandards(IList<int> routeIds, IList<int> levels, StandardFilter filter, bool includeAllProperties, ApprenticeshipType? apprenticeshipType = null);
        Task<IEnumerable<Standard>> GetStandards(string iFateReferenceNumber);
    }
}
