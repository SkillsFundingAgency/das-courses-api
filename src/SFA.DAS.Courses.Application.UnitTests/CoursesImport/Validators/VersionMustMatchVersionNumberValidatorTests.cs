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
    public class VersionMustMatchVersionNumberValidatorTests
    {
        private VersionMustMatchVersionNumberValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new VersionMustMatchVersionNumberValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Version_Matches_VersionNumber_For_Active_Status()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    VersionNumber = new Settable<string>("1.0"),
                    Status = new Settable<string>(Status.ApprovedForDelivery)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Version_Does_Not_Match_VersionNumber_For_Active_Status()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("1.0"),
                    VersionNumber = new Settable<string>("2.0"),
                    Status = new Settable<string>(Status.Retired)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1007: ST1002 version 1.0 should have matching versionNumber");
        }

        [Test]
        public void Should_Not_Add_Failure_When_Status_Is_Not_Active()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1003"),
                    Version = new Settable<string>("1.0"),
                    VersionNumber = new Settable<string>("2.0"),
                    Status = new Settable<string>(Status.InDevelopment)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
