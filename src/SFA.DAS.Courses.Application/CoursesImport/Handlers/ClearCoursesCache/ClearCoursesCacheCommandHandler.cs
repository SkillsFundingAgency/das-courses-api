using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ClearCoursesCache
{
    public class ClearCoursesCacheCommandHandler : IRequestHandler<ClearCoursesCacheCommand>
    {
        private readonly ICoursesCacheService _coursesCacheService;

        public ClearCoursesCacheCommandHandler(ICoursesCacheService coursesCacheService)
        {
            _coursesCacheService = coursesCacheService;
        }

        public async Task Handle(ClearCoursesCacheCommand request, CancellationToken cancellationToken)
        {
            await _coursesCacheService.ClearCoursesCache(
                "manually", cancellationToken);
        }
    }
}
