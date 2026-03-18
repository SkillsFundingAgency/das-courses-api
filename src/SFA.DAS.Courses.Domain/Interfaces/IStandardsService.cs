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
        Task<IEnumerable<Standard>> GetStandardsList(string keyword, IList<int> routeIds, IList<int> levels, OrderBy orderBy, StandardFilter filter, bool includeAllProperties, ApprenticeshipType? apprenticeshipType);
        Task<IEnumerable<Course>> GetCoursesList(string keyword, IList<int> routeIds, IList<int> levels, OrderBy orderBy, StandardFilter filter, bool includeAllProperties, IList<ApprenticeshipType> apprenticeshipTypes, CourseType? courseType = null);
        Task<int> CountStandards(StandardFilter filter = StandardFilter.None);
        Task<int> CountCourses(StandardFilter filter = StandardFilter.None, CourseType? courseType = null);
        Task<Standard> GetStandardByAnyId(string id);
        Task<Course> GetCourseByAnyId(string id);
        Task<IEnumerable<Standard>> GetAllVersionsOfAStandard(string iFateReferenceNumber, CourseType? courseType);
    }
}
