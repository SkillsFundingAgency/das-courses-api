using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly HttpClient _client;

        public DataDownloadService(HttpClient client)
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

    public class DummyDataDownloadService(IConfiguration _configuration) : IDataDownloadService
    {
        public Task<Stream> GetFileStream(string downloadPath)
        {
            downloadPath = _configuration["LarsDataPath"];

            var fileStream = new FileStream(downloadPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

            return Task.FromResult<Stream>(fileStream);
        }
    }
}
