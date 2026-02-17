using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class SkillsEnglandService : ISkillsEnglandService
    {
        private readonly CoursesConfiguration _coursesConfiguration;
        private readonly HttpClient _client;

        public SkillsEnglandService(IOptions<CoursesConfiguration> config, HttpClient client)
        {
            _coursesConfiguration = config.Value;
            _client = client;
        }

        public async Task<SkillsEnglandStandardsResult> GetCourseImports()
        {
            var config = _coursesConfiguration?.SkillsEnglandApiConfiguration;

            var apprenticeshipsTask = GetDataIfConfigured<Apprenticeship>(
                config?.ApprenticeshipsApiUrl);

            var foundationApprenticeshipsTask = GetDataIfConfigured<FoundationApprenticeship>(
                config?.FoundationApprenticeshipsApiUrl);

            var apprenticeshipUnitsTask = GetDataIfConfigured<ApprenticeshipUnit>(
                config?.ApprenticeshipUnitsApiUrl);

            await Task.WhenAll(
                apprenticeshipsTask,
                foundationApprenticeshipsTask,
                apprenticeshipUnitsTask);

            return new SkillsEnglandStandardsResult
            {
                Apprenticeships = apprenticeshipsTask.Result,
                FoundationApprenticeships = foundationApprenticeshipsTask.Result,
                ApprenticeshipUnits = apprenticeshipUnitsTask.Result
            };
        }

        private async Task<IEnumerable<T>> GetDataIfConfigured<T>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return await Task.FromResult(Enumerable.Empty<T>());
            }

            return await GetData<T>(path);
        }

        private async Task<IEnumerable<T>> GetData<T>(string path)
        {
            var response = await _client.GetAsync(path);

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

            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonResponse, settings)
                   ?? Enumerable.Empty<T>();
        }

    }
}
