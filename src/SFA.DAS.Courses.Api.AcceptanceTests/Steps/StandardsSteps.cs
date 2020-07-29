using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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

            model.Standards.Should().BeEquivalentTo(DbUtilities.GetValidTestStandards(), options=> options
                    .Excluding(std=>std.Sector)
                    .Excluding(std=>std.ApprenticeshipFunding)
                    .Excluding(std=>std.LarsStandard)
                    .Excluding(std=>std.RouteId)
                    .Excluding(std=>std.SearchScore)
                );
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
                GetExpected(table), options=> options
                    .WithStrictOrdering()
                    .Excluding(std=>std.Sector)
                    .Excluding(std=>std.ApprenticeshipFunding)
                    .Excluding(std=>std.LarsStandard)
                    .Excluding(std=>std.RouteId)
                    .Excluding(std=>std.SearchScore)
            );
        }

        private IEnumerable<Standard> GetExpected(Table table)
        {
            var existingSectors = DbUtilities.GetTestSectors();
            var standards =  new List<Standard>();
            foreach (var row in table.Rows)
            {
                standards.Add(DbUtilities.GetValidTestStandards().Single(standard => 
                    standard.Title == row["title"] && 
                    standard.RouteId == existingSectors.Single(sector => sector.Route == row["sector"]).Id &&
                    standard.Level == int.Parse(row["level"])));
            }

            return standards;
        }
    }
}
