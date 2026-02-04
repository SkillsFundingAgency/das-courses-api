using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class PreviouslyDefinedRoutesValidatorTests
    {
        private PreviouslyDefinedRoutesValidator _sut;
        private List<Domain.Entities.Route> _currentRoutes;

        [SetUp]
        public void SetUp()
        {
            _currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Name = "Engineering" },
                new Domain.Entities.Route { Name = "Construction" }
            };

            _sut = new PreviouslyDefinedRoutesValidator(_currentRoutes);
        }

        [Test]
        public void Should_Not_Add_Failure_When_Route_Is_Previously_Defined()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    Route = new Settable<string>("Engineering")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_Route_Is_Not_Previously_Defined()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("1.0"),
                    Route = new Settable<string>("Aerospace")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "W1004: ST1002 version 1.0 route 'Aerospace' has not been imported before");
        }
    }
}
