using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class LarsDataDownloadService : ILarsDataDownloadService
    {
        private readonly HttpClient _client;

        public LarsDataDownloadService (HttpClient client)
        {
            _client = client;
        }

        public async Task<Stream> GetFileStream(string downloadPath)
        {
            var response = await _client.GetAsync(downloadPath);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            
            return stream;
        }
    }
}