using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class VersionsSingleApprovedValidatorTests
    {
        private VersionsSingleApprovedValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new VersionsSingleApprovedValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Only_One_Version_Is_Approved()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard { ReferenceNumber = new Settable<string>("ST1001"), Version = new Settable<string>("1.0"), Status = new Settable<string>(Status.ApprovedForDelivery) },
                new Standard { ReferenceNumber = new Settable<string>("ST1001"), Version = new Settable<string>("1.1"), Status = new Settable<string>(Status.Retired) }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Multiple_Versions_Are_Approved()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.0"), Status = new Settable<string>(Status.ApprovedForDelivery) },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.1"), Status = new Settable<string>(Status.ApprovedForDelivery) }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1002: ST1002 versions 1.0, 1.1 only 1 of these should be status 'Approved for delivery'");
        }
    }
}
