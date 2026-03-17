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
    public class LarsCodeIsNumberValidatorTests
    {
        private LarsCodeIsNumberValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeIsNumberValidator();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1001")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Valid(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("12345")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1001")]
        public void Should_Add_Failure_When_LarsCode_Is_Not_A_Number(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("2.0"),
                    LarsCode = Settable<string>.FromInvalidValue("NotANumber")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"S1005: {referenceNumber} version 2.0 larsCode is not a number");
        }

        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_A_Number_For_ApprenticeshipUnit()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = Domain.Entities.ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU1003"),
                    Version = new Settable<string>("2.0"),
                    LarsCode = Settable<string>.FromInvalidValue("NotANumber")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
