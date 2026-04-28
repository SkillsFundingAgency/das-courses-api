using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using Reqnroll;
using SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Entities;

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
        public async Task ThenAllValidCoursesReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            model.Total.Should().Be(DbUtilities.GetValidTestCourses().Count());

            var expectedCourses = new List<Standard>();
            expectedCourses.AddRange(DbUtilities.GetValidTestCourses());

            model.Courses.Should().BeEquivalentTo(expectedCourses.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
        }

        [Then("all valid and invalid courses are returned")]
        public async Task ThenAllValidAndInvalidCoursesAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedCourses = new List<Standard>();
            expectedCourses.AddRange(DbUtilities.GetValidTestCourses());
            expectedCourses.AddRange(DbUtilities.GetInValidTestCourses());

            model.Courses.Should().BeEquivalentTo(expectedCourses.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedCourses.Count);
        }

        [Then("all courses are returned")]
        public async Task ThenAllCoursesAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedCourses = new List<Standard>();
            expectedCourses.AddRange(DbUtilities.GetAllTestCourses());
            
            model.Courses.Should().BeEquivalentTo(expectedCourses.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
            model.Total.Should().Be(expectedCourses.Count);
        }

        [Then("all not yet approved courses are returned")]
        public async Task ThenAllNotApprovedCoursesAreReturned()
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedCourses = new List<Standard>();
            expectedCourses.AddRange(DbUtilities.GetNotYetApprovedTestCourses());

            model.Courses.Should().BeEquivalentTo(expectedCourses.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), FullBaseStandardQueryExludes);
            model.Total.Should().Be(expectedCourses.Count);
        }

        [Then("the following valid courses are returned")]
        public async Task ThenTheFollowingValidCoursesReturned(Table table)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            var model = await HttpUtilities.ReadContent<GetCoursesSearchResponse>(result.Content);

            var expectedCourses = new List<Standard>();
            expectedCourses.AddRange(GetExpected(table));

            model.Courses.Should().BeEquivalentTo(expectedCourses.Select(s => (GetCourseResponse)(Domain.Courses.Course)s), BaseStandardQueryExludes);
        }

        private static IEnumerable<Standard> GetExpected(Table table)
        {
            var testRoutes = DbUtilities.GetTestRoutes();
            var courses = new List<Standard>();
            foreach (var row in table.Rows)
            {
                courses.Add(DbUtilities.GetAllTestCourses().Single(course =>
                    course.Title == row["title"] &&
                    course.RouteCode == testRoutes.Single(sector => sector.Name == row["route"]).Id &&
                    course.Level == int.Parse(row["level"]) &&
                    course.Version == row["version"] &&
                    course.Status == row["status"]));
            }

            return courses;
        }

        /// <summary>
        /// When an api endpoint returns a GetCourseResponse for a Course using a BaseStandardQuery then Options are not included, as the Options are not
        /// returned the Skills which are populated from Options cannot be populated either; CourseDates are derived from versions and can't be tested
        /// here; SearchScore is not relevant.
        /// are populated from 
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Exluded properties which should be configured for the EquivalencyAssertion</returns>
        private static EquivalencyOptions<GetCourseResponse> BaseStandardQueryExludes(EquivalencyOptions<GetCourseResponse> config) =>
            config
                .Excluding(c => c.CourseDates)
                .Excluding(c => c.SearchScore);

        private static EquivalencyOptions<GetCourseResponse> FullBaseStandardQueryExludes(EquivalencyOptions<GetCourseResponse> config) =>
            config
                .Excluding(c => c.SearchScore);
    }
}
