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
    public class FrameworksSteps
    {
        private readonly ScenarioContext _context;

        public FrameworksSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all frameworks are returned")]
        public async Task ThenAllFrameworksReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetFrameworksResponse>(result.Content);
            var expected = DbUtilities.GetFrameworks();

            model.Frameworks.Should().BeEquivalentTo(expected, options => options
                .Excluding(frm => frm.FundingPeriods)
                .Excluding(frm => frm.TypicalLengthFrom)
                .Excluding(frm => frm.TypicalLengthTo)
                .Excluding(frm => frm.TypicalLengthUnit)
            );
        }

        [Then("the framework with id equal to 1 is returned")]
        public async Task ThenFrameworkIsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetFrameworkResponse>(result.Content);
            var expected = DbUtilities.GetFramework();

            model.Should().BeEquivalentTo(expected, options => options
                .Excluding(frm => frm.FundingPeriods)
                .Excluding(frm => frm.TypicalLengthFrom)
                .Excluding(frm => frm.TypicalLengthTo)
                .Excluding(frm => frm.TypicalLengthUnit)
            );
        }
    }
}
