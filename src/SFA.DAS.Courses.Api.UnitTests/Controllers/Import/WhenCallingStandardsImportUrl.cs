using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingStandardsImportUrl
    {
        [Test, MoqAutoData]
        public void Then_Returns_Ok_Result_With_StandardsImportUrl(
            [Greedy] DataLoadController controller)
        {
            var controllerResult = controller.StandardsImportUrl() as OkObjectResult;

            controllerResult.Value.Should().Be(Constants.InstituteOfApprenticeshipsStandardsUrl);
            controllerResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
