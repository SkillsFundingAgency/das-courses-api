using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetRoutes
{
    public class GetRoutesQueryHandler : IRequestHandler<GetRoutesQuery, GetRoutesQueryResult>
    {
        private readonly IRouteService _service;

        public GetRoutesQueryHandler (IRouteService service)
        {
            _service = service;
        }
        public async Task<GetRoutesQueryResult> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
        {
            var routes = await _service.GetRoutes();
            
            return new GetRoutesQueryResult
            {
                Routes = routes
            };
        }
    }
}