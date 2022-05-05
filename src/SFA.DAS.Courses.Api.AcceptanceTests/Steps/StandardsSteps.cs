using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs;
using SFA.DAS.Courses.Domain.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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
            var testRoutes = DbUtilities.GetTestRoutes();
            var standards = new List<Standard>();
            foreach (var row in table.Rows)
            {
                standards.Add(DbUtilities.GetAllTestStandards().Single(standard =>
                    standard.Title == row["title"] &&
                    standard.RouteCode == testRoutes.Single(sector => sector.Name == row["route"]).Id &&
                    standard.Level == int.Parse(row["level"]) &&
                    standard.Version == row["version"] &&
                    standard.Status == row["status"]));
            }

            return standards;
        }

        [Then("the following knowledges are returned")]
        public async Task ThenTheFollowingKnowledgesAreReturned(Table table)
        {
            var knowledges = table.CreateInstance<KsbData>();
            var exp = new GetStandardOptionKsbsResult
            {
                KSBs = knowledges.Knowledge.Select(x => new StandardOptionKsb
                {
                    Type = KsbType.Knowledge,
                    Key = "k1",
                    Detail = x,
                }).ToArray(),
            };

            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var standard = await HttpUtilities.ReadContent<GetStandardOptionKsbsResult>(result.Content);

            standard.Should().BeEquivalentTo(exp);
        }


        private EquivalencyAssertionOptions<Standard> StandardEquivalencyAssertionOptions(EquivalencyAssertionOptions<Standard> config) =>
            config
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Route)
                .Excluding(c => c.RouteCode)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.VersionEarliestStartDate)
                .Excluding(c => c.VersionLatestStartDate)
                .Excluding(c => c.VersionLatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.TrailBlazerContact)
                .Excluding(c => c.Options)
                .Excluding(c => c.EPAChanged)
                .Excluding(c => c.VersionMajor)
                .Excluding(c => c.VersionMinor);
    }

    public class KsbData
    {
        public string[] Knowledge { get; set; }
        public string[] Skills { get; set; }
        public string[] Behaviour { get; set; }
    }
}
