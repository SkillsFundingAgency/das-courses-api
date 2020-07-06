using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingLevels
    {
        [Test, MoqAutoData]
        public void Then_Returns_Levels_From_Levels_Constant(
            LevelsService service)
        {
            var result = service.GetAll();

            result.Should().BeEquivalentTo(LevelsConstant.Levels);
        }
    }
}
