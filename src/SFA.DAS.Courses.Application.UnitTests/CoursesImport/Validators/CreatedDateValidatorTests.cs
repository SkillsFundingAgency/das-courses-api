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
    public class CreatedDateValidatorTests
    {
        private CreatedDateValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CreatedDateValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_CreatedDate_Is_Valid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    CreatedDate = new Settable<System.DateTime>(System.DateTime.UtcNow)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_CreatedDate_Is_Invalid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST002"),
                    Version = new Settable<string>("2.0"),
                    CreatedDate = Settable<System.DateTime>.FromInvalidValue("Invalid Date")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1012: ST002 version 2.0 the createdDate 'Invalid Date' is not a valid date");
        }
    }
}
