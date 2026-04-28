using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourseOptionKsbs;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Courses
{
    public class WhenCallingGetOptionKsbs
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Option_Ksbs_From_Mediator(
            string id,
            string option,
            GetCourseOptionKsbsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseOptionKsbsQuery>(q =>
                        q.Id == id &&
                        q.Option == option),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetOptionKsbs(id, option) as ObjectResult;

            controllerResult.Should().NotBeNull();
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            controllerResult.Value.Should().BeEquivalentTo(queryResult);
        }
    }
}
