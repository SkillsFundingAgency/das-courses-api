using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISkillsEnglandService
    {
        Task<SkillsEnglandStandardsResult> GetCourseImports();
    }
}
