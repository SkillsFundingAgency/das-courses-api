using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourse
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdQueryResult>
    {
        private readonly IStandardsService _standardsService;

        public GetCourseByIdQueryHandler(IStandardsService standardsService)
        {
            ArgumentNullException.ThrowIfNull(standardsService);

            _standardsService = standardsService;
        }

        public async Task<GetCourseByIdQueryResult> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _standardsService.GetCourseByAnyId(request.Id);

            return new GetCourseByIdQueryResult { Course = course };
        }
    }
}
