using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs
{
    public class GetStandardOptionKsbsQueryHandler : IRequestHandler<GetStandardOptionKsbsQuery, GetStandardOptionKsbsResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardOptionKsbsQueryHandler(IStandardsService standardsService)
        {
            ArgumentNullException.ThrowIfNull(standardsService);

            _standardsService = standardsService;
        }

        public async Task<GetStandardOptionKsbsResult> Handle(GetStandardOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            List<Ksb> ksbs = null;

            var standard = await _standardsService.GetStandardByAnyId(request.Id);

            if (standard != null)
            {
                if (request.Option == "all")
                {
                    ksbs = standard.Options
                        .EmptyEnumerableIfNull()
                        .SelectMany(o => o.Ksbs.EmptyEnumerableIfNull())
                        .DistinctBy(k => k.Id)
                        .ToList();
                }
                else
                {
                    ksbs = standard.Options
                        .EmptyEnumerableIfNull()
                        .FirstOrDefault(option =>
                            string.Equals(option.Title, request.Option, StringComparison.OrdinalIgnoreCase))
                        ?.Ksbs;
                }
            }

            return new GetStandardOptionKsbsResult
            {
                Ksbs = ksbs.EmptyEnumerableIfNull().ToArray()
            };
        }
    }
}
