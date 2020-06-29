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
            var stream = await _client.GetStreamAsync(downloadPath);
            
            return stream;
        }
    }
}