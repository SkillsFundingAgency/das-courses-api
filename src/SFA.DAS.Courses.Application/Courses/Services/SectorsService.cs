using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class SectorsService
    {
        private readonly ISectorRepository _repository;

        public SectorsService (ISectorRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Sector>> GetSectors()
        {
            var sectors = await _repository.GetAll();

            return sectors.Select(c => (Sector)c);
        }
    }
}