using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByIdQueryHandler : IRequestHandler<GetStandardByIdQuery, GetStandardByIdResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardByIdQueryHandler(IStandardsService standardsService)
            => _standardsService = standardsService;

        public async Task<GetStandardByIdResult> Handle(GetStandardByIdQuery request, CancellationToken cancellationToken)
        {
            var standard = await _standardsService.GetStandardByAnyId(request.Id, CourseType.Apprenticeship);

            return new GetStandardByIdResult { Standard = standard };
        }
    }
}
