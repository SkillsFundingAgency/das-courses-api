using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public class WhenCallingGetStandardsList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_List_From_Service(
            List<Standard> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            StandardsController controller)
        {
            mockStandardsService
                .Setup(service => service.GetStandardsList())
                .ReturnsAsync(standards);

            var result = await controller.GetList() as OkObjectResult;

            var model = result.Value as GetStandardsListResponse;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Standards.Should().BeEquivalentTo(standards);
        }
    }
}
