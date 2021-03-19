using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Entities;
using TechTalk.SpecFlow;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Steps
{
    [Binding]
    public class OptionsSteps
    {
        private readonly ScenarioContext _context;

        public OptionsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all valid and invalid standards and their options are returned")]
        public async Task ThenAllActiveStandardsAndTheirOptionsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardOptionsListResponse>(result.Content);

            var approvedNewerVersionWithOption = "ST0002_1.0";
            var approvedWithNoOption = "ST0001_1.3";
            var approvedNotAvailableWithOption = "ST0014_1.0";
            var retiredWithNoNewerVersion = "ST0099_1.0";
            var expectedList = new[] { approvedNewerVersionWithOption, approvedWithNoOption, approvedNotAvailableWithOption, retiredWithNoNewerVersion };
            model.StandardOptions.Any(s => expectedList.Contains(s.StandardUId)).Should().BeTrue();

            var retiredWithNewerVersionWithOption = "ST0001_1.2";
            var retiredWithNewerVersionWithNoOption = "ST0001_1.1";
            var withdrawn = "ST0030_1.0";
            var inDevelopment = "ST0016_1.0";
            var excludedList = new[] { retiredWithNewerVersionWithOption, retiredWithNewerVersionWithNoOption, withdrawn, inDevelopment };
            model.StandardOptions.Any(s => !excludedList.Contains(s.StandardUId)).Should().BeTrue();
        }

    }
}
