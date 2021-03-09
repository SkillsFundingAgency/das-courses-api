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
    public class StandardsSteps
    {
        private readonly ScenarioContext _context;

        public StandardsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("all valid standards are returned")]
        public async Task ThenAllValidStandardsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            model.Total.Should().Be(DbUtilities.GetValidTestStandards().Count());

            model.Standards.Should().BeEquivalentTo(DbUtilities.GetValidTestStandards(), StandardEquivalencyAssertionOptions);
        }

        [Then("all valid and invalid standards are returned")]
        public async Task ThenAllValidAndInvalidStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var standardsList = new List<Standard>();
            standardsList.AddRange(DbUtilities.GetValidTestStandards());
            standardsList.AddRange(DbUtilities.GetInValidTestStandards());

            model.Standards.Should().BeEquivalentTo(standardsList, StandardEquivalencyAssertionOptions);
            model.Total.Should().Be(standardsList.Count);
        }

        [Then("all standards are returned")]
        public async Task ThenAllStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var standardsList = new List<Standard>();
            standardsList.AddRange(DbUtilities.GetValidTestStandards());
            standardsList.AddRange(DbUtilities.GetInValidTestStandards());
            standardsList.AddRange(DbUtilities.GetNotYetApprovedTestStandards());
            standardsList.AddRange(DbUtilities.GetWithdrawnStandards());
            standardsList.AddRange(DbUtilities.GetOlderVersionsOfStandards());

            model.Standards.Should().BeEquivalentTo(standardsList, StandardEquivalencyAssertionOptions);
            model.Total.Should().Be(standardsList.Count);
        }

        [Then("all not yet approved standards are returned")]
        public async Task ThenAllNotApprovedStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var standardsList = new List<Standard>();
            standardsList.AddRange(DbUtilities.GetNotYetApprovedTestStandards());

            model.Standards.Should().BeEquivalentTo(standardsList, StandardEquivalencyAssertionOptions);
            model.Total.Should().Be(standardsList.Count);
        }

        [Then("the following valid standards are returned")]
        public async Task ThenTheFollowingValidStandardsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            model.Standards.Should().BeEquivalentTo(
                GetExpected(table), StandardEquivalencyAssertionOptions);
        }

        [Then("the following standard is returned")]
        public async Task ThenTheFollowingStandardIsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var standard = await HttpUtilities.ReadContent<GetStandardResponse>(result.Content);

            standard.Should().BeEquivalentTo(
                GetExpected(table).Single(), StandardEquivalencyAssertionOptions);
        }

        private IEnumerable<Standard> GetExpected(Table table)
        {
            var existingSectors = DbUtilities.GetTestSectors();
            var standards =  new List<Standard>();
            foreach (var row in table.Rows)
            {
                standards.Add(DbUtilities.GetAllTestStandards().Single(standard => 
                    standard.Title == row["title"] && 
                    standard.RouteId == existingSectors.Single(sector => sector.Route == row["sector"]).Id &&
                    standard.Level == int.Parse(row["level"]) &&
                    standard.Version == decimal.Parse(row["version"]) &&
                    standard.Status == row["status"]));
            }

            return standards;
        }

        private EquivalencyAssertionOptions<Standard> StandardEquivalencyAssertionOptions(EquivalencyAssertionOptions<Standard> config) =>
            config
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Sector)
                .Excluding(c => c.RouteId)
                .Excluding(c => c.Route)
                .Excluding(c => c.RouteCode)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.EarliestStartDate)
                .Excluding(c => c.LatestStartDate)
                .Excluding(c => c.LatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.TrailBlazerContact)
                .Excluding(c => c.Options);
    }
}
