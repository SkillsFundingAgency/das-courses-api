using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class QualificationSectorSubjectAreaService : IQualificationSectorSubjectAreaService
    {
        private readonly HttpClient _client;

        public QualificationSectorSubjectAreaService (HttpClient client)
        {
            _client = client;
        }

        public async Task<QualificationItem> GetEntry(string itemHash)
        {
            AddHeader();
            var response =  await _client.GetAsync($"{Constants.QualificationSectorSubjectAreaUrl}items/{itemHash}");
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<QualificationItem>(jsonResponse);
        }

        public async Task<IEnumerable<QualificationItemList>> GetEntries()
        {
            AddHeader();
            var response = await _client.GetAsync($"{Constants.QualificationSectorSubjectAreaUrl}entries/");
            
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<QualificationItemList>>(jsonResponse);
        }

        private void AddHeader()
        {
            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}