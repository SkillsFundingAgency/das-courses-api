using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class InstituteOfApprenticeshipService : IInstituteOfApprenticeshipService
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

        public async Task<IEnumerable<T>> GetStandards<T>()
        {
            var response = await _client.GetAsync(Constants.InstituteOfApprenticeshipsStandardsUrl);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();

            return jsonResponse;
        }
    }
}
