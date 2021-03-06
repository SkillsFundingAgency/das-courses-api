﻿using System;
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
using SFA.DAS.Courses.Application.Courses.Queries.GetFrameworks;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Frameworks
{
    public class WhenCallingGetFrameworks
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_List_From_Mediator(
            GetFrameworksResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] FrameworksController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetFrameworksQuery>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetList() as ObjectResult;

            var model = controllerResult.Value as GetFrameworksResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Frameworks.Should().BeEquivalentTo(queryResult.Frameworks);
            
        }
    }
}
