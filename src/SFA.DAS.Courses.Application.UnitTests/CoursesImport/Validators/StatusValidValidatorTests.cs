using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class StatusValidValidatorTests
    {
        private StatusValidValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new StatusValidValidator();
        }

        [TestCase(Status.ApprovedForDelivery)]
        [TestCase(Status.Retired)]
        [TestCase(Status.Withdrawn)]
        [TestCase(Status.ProposalInDevelopment)]
        [TestCase(Status.InDevelopment)]
        public void Should_Not_Add_Failure_When_Status_Is_Valid(string validStatus)
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
        public void Should_Add_Failure_When_Status_Is_Invalid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("2.0"),
                    Status = new Settable<string>("InvalidStatus")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1009: ST1002 version 2.0 has invalid status 'InvalidStatus'");
        }
    }
}
