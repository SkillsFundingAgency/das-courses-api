using System.Linq;
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
    public class SectorsSteps
    {
        private readonly ScenarioContext _context;

        public SectorsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all sectors are returned")]
        public async Task ThenAllSectorsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetSectorsListResponse>(result.Content);

            model.Sectors.Should().BeEquivalentTo(DbUtilities.GetTestSectors().ToList(), config => 
                config.Excluding(sector => sector.Standards));
        }
    }
}
