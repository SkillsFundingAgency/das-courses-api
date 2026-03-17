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
    public class LarsCodeFormatValidatorTests
    {
        private LarsCodeFormatValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeFormatValidator();
        }

        [TestCase("ZSC00001")]
        [TestCase("ZSC00002")]
        [TestCase("ZSC10001")]

        public void Should_Not_Add_Failure_When_LarsCode_Is_Valid(string larsCode)
        {
            // Arrange
            List<Standard> importedStandards =
            [
                new Standard
                {
                    ApprenticeshipType = Domain.Entities.ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU0001"),
                    LarsCode = larsCode,
                    Version = new Settable<string>("1.0")
                }
            ];

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase("12345")]
        [TestCase("AU000001")]
        public void Should_Add_Failure_When_Lars_Has_Invalid_Format(string larsCode)
        {
            // Arrange
            List<Standard> importedStandards =
            [
                new Standard
                {
                    ApprenticeshipType = Domain.Entities.ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU0001"),
                    LarsCode = larsCode,
                    Version = new Settable<string>("1.0")
                }
            ];

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"E1005: AU0001 version 1.0 of type '{Domain.Entities.ApprenticeshipType.ApprenticeshipUnit}' has not got larsCode in the correct format.");
        }
    }
}
