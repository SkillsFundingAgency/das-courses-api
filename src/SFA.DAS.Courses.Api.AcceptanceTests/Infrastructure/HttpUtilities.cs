using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class HttpUtilities
    {
        public static async Task<T> ReadContent<T>(HttpContent httpContent)
        {
            var json = await httpContent.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
    }
}