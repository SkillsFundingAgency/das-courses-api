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
    public class LarsCodeNotZeroForNewVersionValidatorTests
    {
        private LarsCodeNotZeroForNewVersionValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeNotZeroForNewVersionValidator();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1001")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_Zero_For_New_Version(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.1"),
                    LarsCode = new Settable<string>("12345")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1002")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1002")]
        public void Should_Add_Failure_When_LarsCode_Is_Zero_For_New_Version(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.1"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"S1001: {referenceNumber} version 1.1 has larsCode 0");
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1003")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1003")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_For_Initial_Version(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_For_New_Version_For_ApprenticeshipUnit()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = Domain.Entities.ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU1004"),
                    Version = new Settable<string>("1.1"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
