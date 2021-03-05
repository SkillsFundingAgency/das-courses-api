using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _repository;

        public RouteService (IRouteRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Route>> GetRoutes()
        {
            var routes = await _repository.GetAll();

            return routes.Select(c => (Route) c).ToList();
        }
    }
}