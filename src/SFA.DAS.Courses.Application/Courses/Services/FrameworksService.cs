using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class FrameworksService : IFrameworksService
    {
        private readonly IFrameworkRepository _frameworkRepository;

        public FrameworksService (IFrameworkRepository frameworkRepository)
        {
            _frameworkRepository = frameworkRepository;
        }
        public async Task<Framework> GetFramework(string frameworkId)
        {
            var framework = await _frameworkRepository.Get(frameworkId);

            return framework;
        }

        public async Task<IEnumerable<Framework>> GetFrameworks()
        {
            var frameworks = await _frameworkRepository.GetAll();

            return frameworks.Select(c => (Framework) c).ToList();
        }
    }
}