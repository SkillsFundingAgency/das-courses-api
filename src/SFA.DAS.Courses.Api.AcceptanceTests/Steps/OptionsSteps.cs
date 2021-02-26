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

            var standardsList = new List<Standard>();
            standardsList.AddRange(DbUtilities.GetValidTestStandards());
            standardsList.AddRange(DbUtilities.GetInValidTestStandards());

            model.Standards.Should().BeEquivalentTo(standardsList.Select(standard => new GetStandardOptionsResponse
            {
                StandardUId = standard.StandardUId,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                LarsCode = standard.LarsCode,
                Options = standard.Options
            }).ToList());

            var activeStandardWithOptions = model.Standards.Single(standard => standard.StandardUId == "ST001_1.3");
            activeStandardWithOptions.Options.Count.Should().Be(2);
            activeStandardWithOptions.Options.Contains("Beer").Should().BeTrue();
            activeStandardWithOptions.Options.Contains("Cider").Should().BeTrue();

            var retiredStandardWithOptions = model.Standards.Single(Standard => Standard.StandardUId == "");
            retiredStandardWithOptions.Options.Count.Should().Be(2);
            retiredStandardWithOptions.Options.Contains("Studio").Should().BeTrue();
            retiredStandardWithOptions.Options.Contains("Landscape").Should().BeTrue();

            model.Standards.Where(standard => (standard.Options != null) && standard.Options.Contains("Wine")).Should().BeEmpty();
            model.Standards.Where(standard => (standard.Options != null) && standard.Options.Contains("Ferrous")).Should().BeEmpty();
        }

    }
}
