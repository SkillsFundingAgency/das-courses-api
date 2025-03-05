using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

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

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_Zero()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<int>(12345),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-20))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_LarsCode_Is_Zero_And_Two_Weeks_After_PublishDate()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<int>(0),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-20))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "W1001: ST1002 version 1.0 has a larsCode 0 more than 2 weeks after its publishDate");
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_But_Less_Than_Two_Weeks_After_PublishDate()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1003"),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<int>(0),
                    PublishDate = new Settable<DateTime>(DateTime.Now.AddDays(-10))
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
