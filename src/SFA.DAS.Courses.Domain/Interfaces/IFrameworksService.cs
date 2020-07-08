using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworksService
    {
        Task<Framework> GetFramework(string frameworkId);
        Task<IEnumerable<Framework>> GetFrameworks();
    }
}