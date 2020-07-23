using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetFramework
{
    public class GetFrameworkQueryHandler : IRequestHandler<GetFrameworkQuery,GetFrameworkResult>
    {
        private readonly IFrameworksService _frameworksService;

        public GetFrameworkQueryHandler (IFrameworksService frameworksService)
        {
            _frameworksService = frameworksService;
        }
        public async Task<GetFrameworkResult> Handle(GetFrameworkQuery request, CancellationToken cancellationToken)
        {
            var framework = await _frameworksService.GetFramework(request.FrameworkId);
            
            return new GetFrameworkResult
            {
                Framework = framework
            };
        }
    }
}