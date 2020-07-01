using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetLevels
{
    public class GetLevelsListQueryHandler : IRequestHandler<GetLevelsListQuery, GetLevelsListResult>
    {
        private readonly ILevelsService _levelsService;

        public GetLevelsListQueryHandler(ILevelsService levelsService)
        {
            _levelsService = levelsService;
        }

        public Task<GetLevelsListResult> Handle(GetLevelsListQuery request, CancellationToken cancellationToken)
        {
            var levels = _levelsService.GetAll();

            var result = new GetLevelsListResult {Levels = levels};

            return Task.FromResult(result);
        }
    }
}
