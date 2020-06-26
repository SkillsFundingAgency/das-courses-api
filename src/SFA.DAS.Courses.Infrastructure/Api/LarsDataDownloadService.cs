using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class LarsDataDownloadService
    {
        private readonly HttpClient _client;

        public LarsDataDownloadService (HttpClient client)
        {
            _client = client;
        }

        public async Task<Stream> GetFileStream(string downloadPath)
        {
            var stream = await _client.GetStreamAsync(downloadPath);

            
            return stream;
        }
    }
}