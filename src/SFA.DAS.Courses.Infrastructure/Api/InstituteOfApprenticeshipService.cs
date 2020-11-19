using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
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

        public async Task<Dictionary<string, string>> GetStandardDocuments()
        {
            var response = await _client.GetAsync(Constants.InstituteOfApprenticeshipsStandardsUrl);
            var result = await response.Content.ReadAsStringAsync();
            var rootElement = JsonDocument.Parse(result).RootElement;
            var documents = new Dictionary<string, string>();
            for (var i = 0; i < rootElement.GetArrayLength(); i++)
            {
                var standardElement = rootElement[i];
                var serializedContent = SerializeElement(standardElement);
                var standardReference = standardElement.GetProperty("referenceNumber").GetString();
                var version = standardElement.GetProperty("version").GetString();
                documents.Add(GetKey(standardReference, version), serializedContent);
            }
            return documents;
        }

        private static string SerializeElement(JsonElement element)
        {
            return System.Text.Json.JsonSerializer.Serialize(element, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            });
        }

        private static string GetKey(string referenceNumber, string version)
        {
            if (string.IsNullOrWhiteSpace(version)) version = "xx";
            return $"{referenceNumber}_{version}"; 
        }
    }
}
