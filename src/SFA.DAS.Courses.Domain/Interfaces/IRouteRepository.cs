using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IRouteRepository
    {
        Task InsertMany(IEnumerable<Route> routes);
        void DeleteAll();
        Task<IEnumerable<Route>> GetAll();
    }
}