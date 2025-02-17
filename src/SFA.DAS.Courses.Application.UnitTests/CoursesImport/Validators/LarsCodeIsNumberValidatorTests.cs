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
    public class LarsCodeIsNumberValidatorTests
    {
        private LarsCodeIsNumberValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeIsNumberValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Valid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    LarsCode = new Settable<int>(12345)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_LarsCode_Is_Not_A_Number()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST002"),
                    Version = new Settable<string>("2.0"),
                    LarsCode = Settable<int>.FromInvalidValue("NotANumber")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1005: ST002 version 2.0 larsCode is not a number");
        }
    }
}
