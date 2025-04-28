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
        private readonly CoursesConfiguration _coursesConfiguration;
        private readonly HttpClient _client;

        public InstituteOfApprenticeshipService(IOptions<CoursesConfiguration> config, HttpClient client)
        {
            _coursesConfiguration = config.Value;
            _client = client;
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            var response = await _client.GetAsync(_coursesConfiguration.InstituteOfApprenticeshipsStandardsUrl);
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SettableContractResolver(),
                Converters =
                [
                    new InitializeSettablesJsonConverter()
                ]
            };

            return JsonConvert.DeserializeObject<List<Standard>>(jsonResponse, settings);
        }
    }
}
