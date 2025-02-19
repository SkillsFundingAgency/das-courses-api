using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Domain.Configuration;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingStandardsImportUrl
    {
        private IOptions<CoursesConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _config = Options.Create(new CoursesConfiguration
            {
                InstituteOfApprenticeshipsStandardsUrl = "https://ifate.org"
            });
        }

        [Test]
        public void Then_Returns_Content_Result_With_StandardsImportUrl()
        {
            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<DataLoadController>>();
            var sut = new DataLoadController(mediator.Object, _config, logger.Object);
            
            var controllerResult = sut.StandardsImportUrl() as ContentResult;

            controllerResult.Content.Should().Be(_config.Value.InstituteOfApprenticeshipsStandardsUrl);
            controllerResult.StatusCode.Should().BeNull();
        }
    }
}
