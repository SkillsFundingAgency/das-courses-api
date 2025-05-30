using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.Api
{
    public class InstituteOfApprenticeshipService : IInstituteOfApprenticeshipService
    {
        private readonly CoursesConfiguration _coursesConfiguration;
        private readonly HttpClient _client;

        public InstituteOfApprenticeshipService(IOptions<CoursesConfiguration> config, HttpClient client)
        {
            _coursesConfiguration = config.Value;
            _client = client;
            _client.BaseAddress = new Uri(_coursesConfiguration.InstituteOfApprenticeshipsApiConfiguration.ApiBaseUrl);
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            var standardsTask = GetData(_coursesConfiguration.InstituteOfApprenticeshipsApiConfiguration.StandardsPath, Domain.Entities.ApprenticeshipType.Apprenticeship);
            var foundationApprenticeshipsTask = GetData(_coursesConfiguration.InstituteOfApprenticeshipsApiConfiguration.FoundationApprenticeshipsPath, Domain.Entities.ApprenticeshipType.FoundationApprenticeship);

            await Task.WhenAll(standardsTask, foundationApprenticeshipsTask);

            return standardsTask.Result.Concat(foundationApprenticeshipsTask.Result);
        }

        private async Task<IEnumerable<Standard>> GetData(string path, Domain.Entities.ApprenticeshipType apprenticeshipType)
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

            var standards = JsonConvert.DeserializeObject<IEnumerable<Standard>>(jsonResponse, settings);

            foreach (var standard in standards)
            {
                standard.ApprenticeshipType = apprenticeshipType;
            }

            return standards;
        }
    }
}
