using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;
using TechTalk.SpecFlow;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Steps
{
    [Binding]
    public class LevelsSteps
    {
        private readonly ScenarioContext _context;

        public LevelsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all levels are returned")]
        public async Task ThenAllLevelsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetLevelsListResponse>(result.Content);

            model.Levels.Should().BeEquivalentTo(LevelsConstant.Levels);
        }
    }
}
