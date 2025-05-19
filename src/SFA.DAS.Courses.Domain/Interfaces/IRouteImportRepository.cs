using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IRouteImportRepository
    {
        Task<int> InsertMany(IEnumerable<RouteImport> routes);
        Task<int> DeleteAll();
        Task<IEnumerable<RouteImport>> GetAll();
    }
}
