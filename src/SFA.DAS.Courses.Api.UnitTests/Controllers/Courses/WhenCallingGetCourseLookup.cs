using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourse;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetCourseLookup
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_With_Course_Detail_From_Mediator(
            string id,
            Course course,
            GetCourseByIdQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            // Arrange
            queryResult.Course = course;

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult = await controller.Lookup(id);

            // Assert
            var okResult = controllerResult as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = okResult.Value as GetCourseDetailResponse;
            model.Should().NotBeNull();
            model.Should().BeEquivalentTo((GetCourseDetailResponse)course);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_NotFound_When_Course_Is_Null(
            string id,
            GetCourseByIdQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            // Arrange
            queryResult.Course = null;

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult = await controller.Lookup(id);

            // Assert
            controllerResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
