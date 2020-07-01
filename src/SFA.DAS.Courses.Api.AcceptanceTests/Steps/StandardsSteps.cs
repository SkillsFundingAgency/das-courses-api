using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using SFA.DAS.Courses.Api.ApiResponses;
using TechTalk.SpecFlow;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Steps
{
    [Binding]
    public class StandardsSteps
    {
        private readonly ScenarioContext _context;

        public StandardsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all standards are returned")]
        public async Task ThenAllStandardsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            model.Standards.Should().BeEquivalentTo(DbUtilities.GetTestStandards(),
                config => config.Excluding(standard => standard.SearchScore));
        }
    }
}
