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

            model.Total.Should().Be(standardsList.Count);

            model.Standards.Should().BeEquivalentTo(standardsList, StandardEquivalencyAssertionOptions);
        }

        private EquivalencyAssertionOptions<Standard> StandardEquivalencyAssertionOptions(EquivalencyAssertionOptions<Standard> config) =>
            config
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Sector)
                .Excluding(c => c.RouteId)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.EarliestStartDate)
                .Excluding(c => c.LatestStartDate)
                .Excluding(c => c.LatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.TypicalDuration)
                .Excluding(c => c.MaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.TrailBlazerContact)
                .Excluding(c => c.Status)
                .Excluding(c => c.Title)
                .Excluding(c => c.Level)
                .Excluding(c => c.Version)
                .Excluding(c => c.OverviewOfRole)
                .Excluding(c => c.Keywords)
                .Excluding(c => c.TypicalJobTitles)
                .Excluding(c => c.StandardPageUrl)
                .Excluding(c => c.IntegratedDegree)
                .Excluding(c => c.Skills)
                .Excluding(c => c.Knowledge)
                .Excluding(c => c.Behaviours)
                .Excluding(c => c.Duties)
                .Excluding(c => c.CoreAndOptions)
                .Excluding(c => c.IntegratedApprenticeship);
    }
}
