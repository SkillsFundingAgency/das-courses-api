using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetFrameworks
{
    public class GetFrameworksQueryHandler : IRequestHandler<GetFrameworksQuery, GetFrameworksResult>
    {
        private readonly IFrameworksService _frameworksService;

        public GetFrameworksQueryHandler (IFrameworksService frameworksService)
        {
            _frameworksService = frameworksService;
        }
        
        public async Task<GetFrameworksResult> Handle(GetFrameworksQuery request, CancellationToken cancellationToken)
        {
            var frameworks = await _frameworksService.GetFrameworks();
            
            return new GetFrameworksResult
            {
                Frameworks = frameworks
            }; 
                
        }
    }
}