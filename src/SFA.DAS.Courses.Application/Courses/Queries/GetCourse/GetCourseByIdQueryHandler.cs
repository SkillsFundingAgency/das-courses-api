using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourse
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResult>
    {
        private readonly IStandardsService _standardsService;

        public GetCourseByIdQueryHandler(IStandardsService standardsService)
            => _standardsService = standardsService;

        public async Task<GetCourseByIdResult> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _standardsService.GetCourseByAnyId(request.Id);

            return new GetCourseByIdResult { Course = course };
        }
    }
}
