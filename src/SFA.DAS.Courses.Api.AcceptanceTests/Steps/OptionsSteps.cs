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

            var expectedList = new[] { "ST002_1.0", "ST001_1.3", "ST014_1.0", "ST0099_1.0" };
            model.Standards.Any(s => expectedList.Contains(s.StandardUId)).Should().BeTrue();

            var excludedList = new[] { "ST001_1.2", "ST001_1.1", "ST030_1.0", "ST016_1.0" };
            model.Standards.Any(s => !excludedList.Contains(s.StandardUId)).Should().BeTrue();
        }

    }
}
