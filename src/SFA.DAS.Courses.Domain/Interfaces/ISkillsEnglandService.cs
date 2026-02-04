using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISkillsEnglandService
    {
        Task<IEnumerable<Standard>> GetStandards();
    }
}