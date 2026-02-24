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
    public class TitleValidatorTests
    {
        private TitleValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TitleValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Title_Is_Valid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    Title = new Settable<string>("Valid Title")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Title_Is_Empty()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("2.0"),
                    Title = new Settable<string>("")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1008: ST1002 version 2.0 title must not start with a space and be greater than 1 character");
        }

        [Test]
        public void Should_Add_Failure_When_Title_Starts_With_Space()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1003"),
                    Version = new Settable<string>("3.0"),
                    Title = new Settable<string>(" Invalid Title")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1008: ST1003 version 3.0 title must not start with a space and be greater than 1 character");
        }
    }
}
