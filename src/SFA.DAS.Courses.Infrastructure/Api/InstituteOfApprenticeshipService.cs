using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class InstituteOfApprenticeshipService
    {
        private readonly HttpClient _client;

        public InstituteOfApprenticeshipService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            var response = await _client.GetAsync(Constants.InstituteOfApprenticeshipsStandardsUrl);
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<Standard>>(jsonResponse);
        }
    }
}
