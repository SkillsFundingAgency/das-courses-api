using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Domain.UnitTests.Extensions
{
    public class WhenGettingStandardVersionId
    {
        [Test, AutoData]
        public void Then_the_Reference_And_Version_Are_Combined(string reference, decimal version)
        {
            //Act
            var actual = reference.ToStandardVersionId(version);
            
            //Assert
            actual.Should().Be($"{reference}_{version:0.0}");
        }

        [Test, AutoData]
        public void Then_Whitespace_Is_Trimmed_From_The_Reference(string reference, decimal version)
        {
            //Arrange
            var modified = $"  {reference} ";
            
            //Act
            var actual = modified.ToStandardVersionId(version);
            
            //Assert
            actual.Should().Be($"{reference}_{version:0.0}");
        }

        [Test, AutoData]
        public void Then_If_No_Version_Then_Set_To_One(string reference)
        {
            //Act
            var actual = reference.ToStandardVersionId(null);
            
            //Assert
            actual.Should().Be($"{reference}_1.0");
        }
        
        [Test, AutoData]
        public void Then_If_No_Version_Value_Then_Set_To_One(string reference)
        {
            //Act
            var actual = reference.ToStandardVersionId(0);
            
            //Assert
            actual.Should().Be($"{reference}_1.0");
        }
    }
}