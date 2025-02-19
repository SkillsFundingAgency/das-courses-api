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
    public class VersionsHaveNoGapsValidatorTests
    {
        private VersionsHaveNoGapsValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new VersionsHaveNoGapsValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Versions_Are_Sequential()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard { ReferenceNumber = new Settable<string>("ST1001"), Version = new Settable<string>("1.0") },
                new Standard { ReferenceNumber = new Settable<string>("ST1001"), Version = new Settable<string>("1.1") },
                new Standard { ReferenceNumber = new Settable<string>("ST1001"), Version = new Settable<string>("2.0") }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Version_Minor_Gap_Exists()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.0") },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.2") },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("2.0") }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1003: ST1002 version 1.0 should not be followed by version 1.2");
        }

        [Test]
        public void Should_Add_Failure_When_Version_Major_Gap_Exists()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.0") },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("1.1") },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("2.0") },
                new Standard { ReferenceNumber = new Settable<string>("ST1002"), Version = new Settable<string>("4.0") }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1003: ST1002 version 2.0 should not be followed by version 4.0");
        }
    }
}
