using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsService
    {
        Task<IEnumerable<Standard>> GetStandardsList();
        Task<Standard> GetStandard(int standardId);
    }
}
