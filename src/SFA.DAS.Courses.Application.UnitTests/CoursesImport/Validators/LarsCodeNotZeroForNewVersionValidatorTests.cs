using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class LarsCodeNotResetToZeroValidatorTests
    {
        private LarsCodeNotResetToZeroValidator _sut;
        private List<Domain.Entities.Standard> _currentStandards;

        [SetUp]
        public void SetUp()
        {
            _currentStandards = new List<Domain.Entities.Standard>();
            _sut = new LarsCodeNotResetToZeroValidator(_currentStandards);
        }

        [TestCase(ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA1001")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_Zero(
            ApprenticeshipType apprenticeshipType,
            string referenceNumber)
        {
            // Arrange
            _currentStandards.Add(new Domain.Entities.Standard
            {
                IfateReferenceNumber = referenceNumber,
                LarsCode = "12345"
            });

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

        [TestCase(ApprenticeshipType.Apprenticeship, "ST1002")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA1002")]
        public void Should_Add_Failure_When_LarsCode_Is_Reset_To_Zero(
            ApprenticeshipType apprenticeshipType,
            string referenceNumber)
        {
            // Arrange
            _currentStandards.Add(new Domain.Entities.Standard
            {
                IfateReferenceNumber = referenceNumber,
                LarsCode = "12345"
            });

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
                $"S1001: {referenceNumber} version 1.1 has larsCode 0 but an existing version of this standard has a non-zero larsCode");
        }

        [TestCase(ApprenticeshipType.Apprenticeship, "ST1003")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA1003")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_And_No_Current_Standard_Exists(
            ApprenticeshipType apprenticeshipType,
            string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("2.0"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(ApprenticeshipType.Apprenticeship, "ST1004")]
        [TestCase(ApprenticeshipType.FoundationApprenticeship, "FA1004")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_And_Current_Standard_Also_Has_Zero_LarsCode(
            ApprenticeshipType apprenticeshipType,
            string referenceNumber)
        {
            // Arrange
            _currentStandards.Add(new Domain.Entities.Standard
            {
                IfateReferenceNumber = referenceNumber,
                LarsCode = "0"
            });

            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("2.0"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_For_ApprenticeshipUnit()
        {
            // Arrange
            _currentStandards.Add(new Domain.Entities.Standard
            {
                IfateReferenceNumber = "AU1005",
                LarsCode = "12345"
            });

            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU1005"),
                    Version = new Settable<string>("1.1"),
                    LarsCode = new Settable<string>("0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Current_Standard_Has_Non_Zero_LarsCode_For_Different_ReferenceNumber()
        {
            // Arrange
            _currentStandards.Add(new Domain.Entities.Standard
            {
                IfateReferenceNumber = "ST9999",
                LarsCode = "12345"
            });

            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = ApprenticeshipType.Apprenticeship,
                    ReferenceNumber = new Settable<string>("ST1006"),
                    Version = new Settable<string>("2.0"),
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
