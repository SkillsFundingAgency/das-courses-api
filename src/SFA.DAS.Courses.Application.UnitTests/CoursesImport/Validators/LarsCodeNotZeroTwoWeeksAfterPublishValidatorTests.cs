using System;
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
    public class LarsCodeNotZeroTwoWeeksAfterPublishValidatorTests
    {
        private LarsCodeNotZeroTwoWeeksAfterPublishValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeNotZeroTwoWeeksAfterPublishValidator();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1001")]
        [TestCase(Domain.Entities.ApprenticeshipType.ApprenticeshipUnit, "AU1001")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_Zero(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("12345"),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-20))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1002")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1002")]
        public void Should_Add_Failure_When_LarsCode_Is_Zero_And_Two_Weeks_After_PublishDate(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("0"),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-20))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                $"W1001: {referenceNumber} version 1.0 has a larsCode 0 more than 2 weeks after its publishDate");
        }

        [TestCase(Domain.Entities.ApprenticeshipType.Apprenticeship, "ST1003")]
        [TestCase(Domain.Entities.ApprenticeshipType.FoundationApprenticeship, "FA1003")]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_But_Less_Than_Two_Weeks_After_PublishDate(Domain.Entities.ApprenticeshipType apprenticeshipType, string referenceNumber)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = apprenticeshipType,
                    ReferenceNumber = new Settable<string>(referenceNumber),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("0"),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-10))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_And_Two_Weeks_After_PublishDate_For_ApprenticeshipUnit()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ApprenticeshipType = Domain.Entities.ApprenticeshipType.ApprenticeshipUnit,
                    ReferenceNumber = new Settable<string>("AU10004"),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<string>("0"),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-20))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
