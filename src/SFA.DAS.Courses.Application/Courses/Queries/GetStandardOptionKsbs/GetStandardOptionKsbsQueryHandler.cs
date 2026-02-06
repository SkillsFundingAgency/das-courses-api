using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.Interfaces;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs
{
    public class GetStandardOptionKsbsQueryHandler : IRequestHandler<GetStandardOptionKsbsQuery, GetStandardOptionKsbsResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardOptionKsbsQueryHandler(IStandardsService standardsService)
            => _standardsService = standardsService;

        public async Task<GetStandardOptionKsbsResult> Handle(GetStandardOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            List<Ksb> ksbs = null;

            var standard = await _standardsService.GetStandardByAnyId(request.Id, CourseType.Apprenticeship);

            if (standard != null)
            {
                ksbs = standard.Options.FirstOrDefault(x => x.Title == request.Option)?.Ksbs;
            }

            return new GetStandardOptionKsbsResult
            {
                Ksbs = ksbs.EmptyEnumerableIfNull().ToArray()
            };
        }
    }
}
