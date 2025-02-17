using AutoFixture.NUnit3;
using FluentAssertions;
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
        public void Then_Returns_Content_Result_With_StandardsImportUrl(
            [Greedy] DataLoadController controller)
        {
            var controllerResult = controller.StandardsImportUrl() as ContentResult;

            controllerResult.Content.Should().Be(Constants.InstituteOfApprenticeshipsStandardsUrl);
            controllerResult.StatusCode.Should().BeNull();
        }
    }
}
