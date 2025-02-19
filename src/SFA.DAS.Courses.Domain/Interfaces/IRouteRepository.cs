using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IRouteRepository
    {
        Task<int> InsertMany(IEnumerable<Route> routes);
        Task DeleteAll();
        Task<IEnumerable<Route>> GetAll();
    }
}
