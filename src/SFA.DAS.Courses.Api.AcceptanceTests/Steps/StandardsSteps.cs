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

        [When("I get a standard with a core pseudo-option")]
        public Task WhenIGetAStandardWithACorePseudo_Option()
        {
            return new HttpSteps(_context).WhenIMethodTheFollowingUrl(
                Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Get,
                "/api/courses/standards/2");
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

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetValidTestStandards());

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetStandardResponse)(Domain.Courses.Standard)s), BaseStandardQueryExludes);
        }

        [Then("all valid and invalid standards are returned")]
        public async Task ThenAllValidAndInvalidStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetValidTestStandards());
            expectedStandards.AddRange(DbUtilities.GetInValidTestStandards());

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetStandardResponse)(Domain.Courses.Standard)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("all standards are returned")]
        public async Task ThenAllStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetAllTestStandards());
            
            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetStandardResponse)(Domain.Courses.Standard)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("all not yet approved standards are returned")]
        public async Task ThenAllNotApprovedStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetNotYetApprovedTestStandards());

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetStandardResponse)(Domain.Courses.Standard)s), FullBaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("the following valid standards are returned")]
        public async Task ThenTheFollowingValidStandardsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardsListResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(GetExpected(table));

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetStandardResponse)(Domain.Courses.Standard)s), BaseStandardQueryExludes);
        }

        [Then("the following valid standard versions are returned")]
        public async Task ThenTheFollowingValidStandardVersionsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            // a versions request using the BaseQuery which does not get options, so it will not include Options, Skills or Ksbs

            var model = await HttpUtilities.ReadContent<GetStandardVersionsListResponse>(result.Content);

            var expectedStandards = new List<Standard>(GetExpected(table));

            model.Standards.Should().BeEquivalentTo(
                expectedStandards.Select(s => (GetStandardDetailResponse)(Domain.Courses.Standard)s), BaseStandardQueryExcludes);
        }

        [Then("the following standard is returned")]
        public async Task ThenTheFollowingStandardIsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetStandardDetailResponse>(result.Content);

            var expectedStandard = GetExpected(table).Single();

            model.Should().BeEquivalentTo((GetStandardDetailResponse)(Domain.Courses.Standard)expectedStandard, FullBaseStandardQueryExludes);
        }

        private static IEnumerable<Standard> GetExpected(Table table)
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
            var ksbs = table.CreateSet<Domain.Courses.Ksb>().ToArray();

            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var standard = await HttpUtilities.ReadContent<GetStandardOptionKsbsResult>(result.Content);

            standard.Ksbs.Should().BeEquivalentTo(ksbs, opts => opts.Excluding(f => f.Id));
        }

        [Then("the returned standard has no options")]
        public async Task ThenTheReturnedStandardHasNoOptions()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var standard = await HttpUtilities.ReadContent<GetStandardDetailResponse>(result.Content);

            standard.Options.Should().BeEmpty();
        }

        /// <summary>
        /// When an api endpoint returns a GetStandardDetailResponse for a Standard using a BaseStandardQuery then Options are not included, the Options
        /// are returned but will not be populated, so Skills and Ksbs cannot be populated either; SearchScore is not relevant.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Exluded properties which should be configured for the EquivalencyAssertion</returns>
        private static EquivalencyAssertionOptions<GetStandardDetailResponse> BaseStandardQueryExcludes(EquivalencyAssertionOptions<GetStandardDetailResponse> config) =>
            config
                .Excluding(c => c.Ksbs)
                .Excluding(c => c.Options)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Skills);

        /// <summary>
        /// When an api endpoint returns a GetStandardResponse for a Standard using a BaseStandardQuery then Options are not included, the Options are not
        /// returned but the Skills which are populated from Options cannot be populated either; SearchScore is not relevant.
        /// are populated from 
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Exluded properties which should be configured for the EquivalencyAssertion</returns>
        private static EquivalencyAssertionOptions<GetStandardResponse> BaseStandardQueryExludes(EquivalencyAssertionOptions<GetStandardResponse> config) =>
            config
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Skills);

        private static EquivalencyAssertionOptions<GetStandardDetailResponse> FullBaseStandardQueryExludes(EquivalencyAssertionOptions<GetStandardDetailResponse> config) =>
            config
                .Excluding(c => c.SearchScore);

        private static EquivalencyAssertionOptions<GetStandardResponse> FullBaseStandardQueryExludes(EquivalencyAssertionOptions<GetStandardResponse> config) =>
            config
                .Excluding(c => c.SearchScore);
    }
}
