using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.Courses;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class StatusRecommendedValidatorTests
    {
        private StatusRecommendedValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new StatusRecommendedValidator();
        }

        [TestCase(Status.ApprovedForDelivery)]
        [TestCase(Status.Retired)]
        [TestCase(Status.Withdrawn)]
        public void Should_Not_Add_Failure_When_Status_Is_Recommended(string validStatus)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    Status = new Settable<string>(validStatus)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Status_Is_Not_Recommended_For_Version_1_0()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("1.0"),
                    Status = new Settable<string>("In Development")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "W1002: ST1002 version 1.0 has status 'In Development'");
        }
    }
}
