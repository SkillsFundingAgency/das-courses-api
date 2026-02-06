using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardRepository
    {
        Task<int> Count(StandardFilter filter, CourseType? courseType);
        Task DeleteAll();
        Task<int> InsertMany(IEnumerable<Standard> standards);
        Task<Standard> GetLatestActiveStandard(string larsCode, CourseType? courseType);
        Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber, CourseType? courseType);
        Task<List<Standard>> GetActiveStandardsByIfateReferenceNumbers(List<string> ifateReferenceNumbers, CourseType? courseType);
        Task<Standard> Get(string standardUId, CourseType? courseType);
        Task<IEnumerable<Standard>> GetStandards(CourseType? courseType);
        Task<IEnumerable<Standard>> GetStandards(IList<int> routeIds, IList<int> levels, StandardFilter filter, bool includeAllProperties, ApprenticeshipType? apprenticeshipType = null, CourseType? courseType = null);
        Task<IEnumerable<Standard>> GetStandards(string iFateReferenceNumber, CourseType? courseType);
    }
}
