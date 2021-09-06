using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Domain.UnitTests.Extensions
{
    public class WhenGettingVersion
    {
        [Test, AutoData]
        public void AndVersionIsNull_Then_Return_Default_Version_Value()
        {
            //Arrange
            string version = null;

            //Act
            var actual = version.ToBaselineVersion();

            //Assert
            actual.Should().Be("1.0");
        }

        [Test, AutoData]
        public void AndVersionIsWhitespace_Then_Return_Default_Version_Value()
        {
            //Arrange
            var version = "   ";

            //Act
            var actual = version.ToBaselineVersion();

            //Assert
            actual.Should().Be("1.0");
        }

        [Test, AutoData]
        public void AndVersionIsNotADecimal_Then_Return_Default_Version_Value()
        {
            //Arrange
            var version = "notaversion";

            //Act
            var actual = version.ToBaselineVersion();

            //Assert
            actual.Should().Be("1.0");
        }

        [Test, AutoData]
        public void AndVersionIsLessThanOne_Then_Return_Default_Version_Value()
        {
            //Arrange
            var version = "-12";

            //Act
            var actual = version.ToBaselineVersion();

            //Assert
            actual.Should().Be("1.0");
        }
    }
}
