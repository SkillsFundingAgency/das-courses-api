using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
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

        [TestCase(ApprenticeshipType.Apprenticeship, "ST0001")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA0002")]
        public void Should_Not_Add_Failure_When_Version_Matches_VersionNumber_For_Active_Status(
            ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
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

        [TestCase(ApprenticeshipType.Apprenticeship, "ST0001")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA0002")]
        public void Should_Add_Failure_When_Version_Does_Not_Match_VersionNumber_For_Active_Status(
            ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    VersionNumber = new Settable<string>("2.0"),
                    Status = new Settable<string>(Status.Retired)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"S1007: {referenceNumber} version 1.0 should have matching versionNumber");
        }

        [TestCase(ApprenticeshipType.Apprenticeship, "ST0001")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA0001")]
        public void Should_Not_Add_Failure_When_Status_Is_Not_Active(
            ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
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

        [Test]
        public void Should_Not_Add_Failure_When_Version_Does_Not_Match_VersionNumber_For_Active_Status_For_ApprenticeshipUnit()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU0001"),
                    Version = new Settable<string>("1.0"),
                    VersionNumber = new Settable<string>("2.0"),
                    Status = new Settable<string>(Status.Retired)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
