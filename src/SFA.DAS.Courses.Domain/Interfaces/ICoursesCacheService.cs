using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ICoursesCacheService
    {
        Task ClearCoursesCache(string reason, CancellationToken cancellationToken);
    }
}
