using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISectorService
    {
        Task<IEnumerable<Sector>> GetSectors();
    }
}