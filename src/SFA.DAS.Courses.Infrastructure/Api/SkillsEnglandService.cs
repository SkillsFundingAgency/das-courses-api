using System;
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
            _client.BaseAddress = new Uri(_coursesConfiguration.SkillsEnglandApiConfiguration.ApiBaseUrl);
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            var apprenticeshipsTask = GetData<Apprenticeship>(_coursesConfiguration.SkillsEnglandApiConfiguration.StandardsPath);
            var foundationApprenticeshipsTask = GetData<FoundationApprenticeship>(_coursesConfiguration.SkillsEnglandApiConfiguration.FoundationApprenticeshipsPath);
            var apprenticeshipUnitsTask = GetData<ApprenticeshipUnit>(_coursesConfiguration.SkillsEnglandApiConfiguration.ApprenticeshipUnitsPath);

            await Task.WhenAll(apprenticeshipsTask, foundationApprenticeshipsTask, apprenticeshipUnitsTask);

            var apprenticeships = apprenticeshipsTask.Result.Select(p => (Standard)p);
            var foundationApprenticeships = foundationApprenticeshipsTask.Result.Select(p => (Standard)p);
            var apprenticeshipUnits = apprenticeshipUnitsTask.Result.Select(p => (Standard)p);

            return apprenticeships
                .Concat(foundationApprenticeships)
                .Concat(apprenticeshipUnits);
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

            var standards = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonResponse, settings);
            return standards;
        }
    }
}
