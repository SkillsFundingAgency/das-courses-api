using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class InstituteOfApprenticeshipService : IInstituteOfApprenticeshipService
    {
        private readonly HttpClient _client;
        private readonly CoursesConfiguration _coursesConfiguration;

        public InstituteOfApprenticeshipService(HttpClient client, IOptions<CoursesConfiguration> config)
        {
            _client = client;
            _coursesConfiguration = config.Value;
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            var response = await _client.GetAsync(_coursesConfiguration.InstituteOfApprenticeshipsStandardsUrl);
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SettableContractResolver()
            };

            return JsonConvert.DeserializeObject<List<Standard>>(jsonResponse, settings);
        }
    }
}
