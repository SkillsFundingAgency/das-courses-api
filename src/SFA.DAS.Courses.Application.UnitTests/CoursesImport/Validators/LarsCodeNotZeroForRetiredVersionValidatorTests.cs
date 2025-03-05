using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class LarsCodeNotZeroForRetiredVersionValidatorTests
    {
        private LarsCodeNotZeroForRetiredVersionValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LarsCodeNotZeroForRetiredVersionValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Not_Zero_For_Retired_Version()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("2.0"),
                    LarsCode = new Settable<int>(12345),
                    Status = new Settable<string>(Status.Retired)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_LarsCode_Is_Zero_For_Retired_Version()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("2.0"),
                    LarsCode = new Settable<int>(0),
                    Status = new Settable<string>(Status.Retired)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1006: ST1002 version 2.0 should not have a larsCode 0 when status is 'Retired'");
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Zero_For_Non_Retired_Version()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1003"),
                    Version = new Settable<string>("2.0"),
                    LarsCode = new Settable<int>(0),
                    Status = new Settable<string>(Status.ApprovedForDelivery)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
