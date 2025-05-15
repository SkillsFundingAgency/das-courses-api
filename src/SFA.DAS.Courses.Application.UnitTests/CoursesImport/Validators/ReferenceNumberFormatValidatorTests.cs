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
    public class ReferenceNumberFormatValidatorTests
    {
        private ReferenceNumberFormatValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ReferenceNumberFormatValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_ReferenceNumber_Is_Valid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_ReferenceNumber_Has_Invalid_Format()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("INVALID123"),
                    Version = new Settable<string>("1.0")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "E1002: INVALID123 version 1.0 referenceNumber is not in the correct format.");
        }
    }
}
