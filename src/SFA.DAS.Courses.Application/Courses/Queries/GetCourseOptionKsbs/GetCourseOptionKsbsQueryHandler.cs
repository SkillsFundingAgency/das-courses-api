using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourseOptionKsbs
{
    public class GetCourseOptionKsbsQueryHandler : IRequestHandler<GetCourseOptionKsbsQuery, GetCourseOptionKsbsResult>
    {
        private readonly IStandardsService _standardsService;

        public GetCourseOptionKsbsQueryHandler(IStandardsService standardsService)
        {
            ArgumentNullException.ThrowIfNull(standardsService);

            _standardsService = standardsService;
        }

        public async Task<GetCourseOptionKsbsResult> Handle(GetCourseOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            List<Ksb> ksbs = null;

            var course = await _standardsService.GetCourseByAnyId(request.Id);

            if (course != null)
            {
                if (request.Option == "all")
                {
                    ksbs = course.Options
                        .EmptyEnumerableIfNull()
                        .SelectMany(o => o.Ksbs.EmptyEnumerableIfNull())
                        .DistinctBy(k => k.Id)
                        .ToList();
                }
                else
                {
                    ksbs = course.Options
                        .EmptyEnumerableIfNull()
                        .FirstOrDefault(option =>
                            option.Title.Equals(request.Option, StringComparison.OrdinalIgnoreCase))
                        ?.Ksbs;
                }
            }

            return new GetCourseOptionKsbsResult
            {
                Ksbs = ksbs.EmptyEnumerableIfNull().ToArray()
            };
        }
    }
}
