using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class StandardsService : IStandardsService
    {
        private readonly IStandardRepository _standardsRepository;

        public StandardsService(IStandardRepository standardsRepository)
        {
            _standardsRepository = standardsRepository;
        }

        public async Task<IEnumerable<Standard>> GetStandardsList(string keyword)
        {
            var standards = await _standardsRepository.GetAll();

            return standards.Select(standard => (Standard)standard).ToList();
        }

        public async Task<Standard> GetStandard(int standardId)
        {
            var standard = await _standardsRepository.Get(standardId);

            return standard;
        }
    }
}
