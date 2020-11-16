using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using AutoFixture.NUnit3;
using Castle.Core.Internal;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardApiTypeToStandardImport
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(ImportTypes.Standard standard)
        {
            var actual = (StandardImport)standard;

            actual.Should().BeEquivalentTo(standard, options => options
                 .Excluding(c => c.Route)
                 .Excluding(c => c.Duties)
                 .Excluding(c => c.Keywords)
                 .Excluding(c => c.Skills)
                 .Excluding(c => c.Knowledge)
                 .Excluding(c => c.Behaviours)
                 .Excluding(c => c.JobRoles)
                 .Excluding(c => c.LarsCode)
                 .Excluding(c => c.Status)
                 .Excluding(c => c.StandardPageUrl)
                 .Excluding(c => c.TypicalJobTitles)
                 .Excluding(c => c.CoreAndOptions)
            );

            actual.Id.Should().Be(standard.LarsCode);
            actual.StandardPageUrl.Should().Be(standard.StandardPageUrl.AbsoluteUri);            
            actual.TypicalJobTitles.Should().Be(string.Join("|", standard.TypicalJobTitles));
        }

        [Test, AutoData]
        public void Then_Leading_And_Trailing_Whitespace_Is_Removed_From_The_Course_Title(ImportTypes.Standard standard)
        {
            //Arrange
            var expectedTitle = standard.Title;
            standard.Title = $"  {expectedTitle} ";
            
            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Title.Should().Be(expectedTitle);
        }

        [Test, AutoData]
        public void Then_If_The_Version_Is_Null_It_Is_Set_As_Zero(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Version = null;            

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Version.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Skills.Should().BeEquivalentTo(standard.Skills.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_All_Knowledge_Is_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Knowledge.Should().BeEquivalentTo(standard.Knowledge.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_All_Behaviours_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Behaviours.Should().BeEquivalentTo(standard.Behaviours.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_Mappings_Cope_With_Null_Sources(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Knowledge = null;
            standard.Behaviours = null;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Knowledge.Should().BeEmpty();
            actual.Behaviours.Should().BeEmpty();
        }
    }
}
