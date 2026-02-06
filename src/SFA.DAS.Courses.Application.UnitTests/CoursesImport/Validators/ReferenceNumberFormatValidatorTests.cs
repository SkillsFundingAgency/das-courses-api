using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class ReferenceNumberFormatValidatorTests
    {
        private ReferenceNumberFormatValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ReferenceNumberFormatValidator();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA0001")]
        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, " ST1001 ", Reason = "Spaces around the reference number should be ignored as they are going to be trimmed")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, " FA0001 ")]
        public void Should_Not_Add_Failure_When_ReferenceNumber_Is_Valid(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            List<Standard> importedStandards =
            [
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0")
                }
            ];

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "FA0001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "INVALID123")]
        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "INVALID123")]
        public void Should_Add_Failure_When_ReferenceNumber_Has_Invalid_Format(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            List<Standard> importedStandards =
            [
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0")
                }
            ];

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"E1002: {referenceNumber} version 1.0 of type '{apprenticeshipType}' has not got referenceNumber in the correct format.");
        }
    }
}
