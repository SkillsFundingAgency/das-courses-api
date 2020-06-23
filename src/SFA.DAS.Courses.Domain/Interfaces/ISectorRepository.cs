using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorRepository
    {
        Task InsertMany(IEnumerable<Sector> sector);
        Task<IEnumerable<Sector>> GetAll();
    }
}