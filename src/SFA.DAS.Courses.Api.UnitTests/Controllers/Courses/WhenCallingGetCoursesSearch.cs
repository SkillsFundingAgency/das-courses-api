using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.Courses.Queries.GetCoursesSearch;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetCoursesSearch
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_Search_From_Mediator(
            List<int> routeIds,
            List<int> levels,
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            ApprenticeshipType learningType,
            CourseType courseType,
            GetCoursesSearchQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            // Arrange
            filter = StandardFilter.None;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCoursesSearchQuery>(query =>
                        query.Keyword == keyword &&
                        query.RouteIds.SequenceEqual(routeIds) &&
                        query.Levels.SequenceEqual(levels) &&
                        query.OrderBy.Equals(orderBy) &&
                        query.Filter.Equals(filter) &&
                        !query.IncludeAllProperties &&
                        query.LearningType.Equals(learningType) &&
                        query.CourseType.Equals(courseType)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult = await controller.Search(keyword, routeIds, levels, learningType, courseType, orderBy, filter) as ObjectResult;

            // Assert
            var model = controllerResult.Value as GetCoursesSearchResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Total.Should().Be(queryResult.Total);
            model.TotalFiltered.Should().Be(queryResult.TotalFiltered);

            // Assert all properties except LarsCode
            model.Courses.Should().BeEquivalentTo(
                queryResult.Standards,
                options => CoursesEquivalencyAssertionOptions
                    .GetCourseResponseExclusions(options));
        }
    }
}
