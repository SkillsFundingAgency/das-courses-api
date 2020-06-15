using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IInstituteOfApprenticeshipService
    {
        Task<IEnumerable<Standard>> GetStandards();
    }
}