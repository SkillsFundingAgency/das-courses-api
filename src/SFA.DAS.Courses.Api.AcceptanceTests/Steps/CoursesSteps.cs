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
    public class CoursesSteps
    {
        private readonly ScenarioContext _context;

        public CoursesSteps(ScenarioContext context)
        {
            _context = context;
        }


        [Then("all valid courses are returned")]
        public async Task ThenAllValidStandardsReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            model.Total.Should().Be(DbUtilities.GetValidTestStandards(null).Count());

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetValidTestStandards(null));

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
        }

        [Then("all valid and invalid courses are returned")]
        public async Task ThenAllValidAndInvalidStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetValidTestStandards(null));
            expectedStandards.AddRange(DbUtilities.GetInValidTestStandards());

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("all courses are returned")]
        public async Task ThenAllStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetAllTestStandards(null));
            
            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("all not yet approved courses are returned")]
        public async Task ThenAllNotApprovedStandardsAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(DbUtilities.GetNotYetApprovedTestStandards());

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), FullBaseStandardQueryExludes);
            model.Total.Should().Be(expectedStandards.Count);
        }

        [Then("the following valid courses are returned")]
        public async Task ThenTheFollowingValidStandardsReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(GetExpected(table));

            model.Standards.Should().BeEquivalentTo(expectedStandards.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
        }

        private static IEnumerable<Standard> GetExpected(Table table)
        {
            var testRoutes = DbUtilities.GetTestRoutes();
            var standards = new List<Standard>();
            foreach (var row in table.Rows)
            {
                standards.Add(DbUtilities.GetAllTestStandards(null).Single(standard =>
                    standard.Title == row["title"] &&
                    standard.RouteCode == testRoutes.Single(sector => sector.Name == row["route"]).Id &&
                    standard.Level == int.Parse(row["level"]) &&
                    standard.Version == row["version"] &&
                    standard.Status == row["status"]));
            }

            return standards;
        }

        /// <summary>
        /// When an api endpoint returns a GetCourseResponse for a Course using a BaseStandardQuery then Options are not included, the Options are not
        /// returned but the Skills which are populated from Options cannot be populated either; SearchScore is not relevant.
        /// are populated from 
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Exluded properties which should be configured for the EquivalencyAssertion</returns>
        private static EquivalencyAssertionOptions<GetCourseResponse> BaseStandardQueryExludes(EquivalencyAssertionOptions<GetCourseResponse> config) =>
            config
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Skills);

        private static EquivalencyAssertionOptions<GetCourseResponse> FullBaseStandardQueryExludes(EquivalencyAssertionOptions<GetCourseResponse> config) =>
            config
                .Excluding(c => c.SearchScore);
    }
}
