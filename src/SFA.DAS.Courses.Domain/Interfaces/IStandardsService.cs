using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Search;

using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;
using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsService
    {
        Task<IEnumerable<Standard>> GetStandardsList(string keyword, IList<int> routeIds, IList<int> levels, OrderBy orderBy, StandardFilter filter, bool includeAllProperties, ApprenticeshipType? apprenticeship, CourseType? courseType);
        Task<IEnumerable<Standard>> GetAllVersionsOfAStandard(string iFateReferenceNumber, CourseType? courseType);
        Task<int> Count(StandardFilter filter = StandardFilter.None, CourseType? courseType = null);
        Task<Standard> GetLatestActiveStandard(string larsCode, CourseType? courseType);
        Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber, CourseType? courseType);
        Task<Standard> GetStandard(string standardUId, CourseType? courseType);
        Task<Standard> GetStandardByAnyId(string id, CourseType? courseType);
    }
}
