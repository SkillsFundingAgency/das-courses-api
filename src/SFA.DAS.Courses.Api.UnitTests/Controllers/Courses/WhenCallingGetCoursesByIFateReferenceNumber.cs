using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses.Courses.All;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.Courses.Queries.GetCoursesByIFateReference;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetCoursesByIFateReferenceNumber
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_CourseVersions_List_From_Mediator(
            string iFateReferenceNumber,
            GetCoursesByIFateReferenceResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            // Arrange
            var expectedLarsCodes = queryResult.Courses
                .Select((course, index) =>
                {
                    course.LarsCode = (1000 + index).ToString();
                    return course.LarsCode;
                })
                .ToList();

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCoursesByIFateReferenceQuery>(q =>
                        q.IFateReferenceNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult =
                await controller.GetCoursesByIFateReferenceNumber(iFateReferenceNumber) as ObjectResult;

            // Assert
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetCourseVersionsListResponse;

            model.Courses.Should().BeEquivalentTo(
                queryResult.Courses,
                options => CoursesEquivalencyAssertionOptions
                    .GetCourseResponseExclusions(options)
                    .Excluding(s => s.LarsCode));

            model.Courses
                .Select(s => s.LarsCode)
                .Should()
                .BeEquivalentTo(expectedLarsCodes, o => o.WithStrictOrdering());
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_NotFound_When_No_Courses_Are_Returned(
            string iFateReferenceNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            // Arrange
            var queryResult = new GetCoursesByIFateReferenceResult
            {
                Courses = []
            };

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCoursesByIFateReferenceQuery>(q =>
                        q.IFateReferenceNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult =
                await controller.GetCoursesByIFateReferenceNumber(iFateReferenceNumber);

            // Assert
            controllerResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
