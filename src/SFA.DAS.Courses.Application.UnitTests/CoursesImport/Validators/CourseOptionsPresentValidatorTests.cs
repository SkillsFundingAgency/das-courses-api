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
    public class CourseOptionsPresentValidatorTests
    {
        private CourseOptionsPresentValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CourseOptionsPresentValidator();
        }

        [Test]
        public void Should_Not_Add_Failure_When_Either_Options_Are_Present()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    CoreAndOptions = new Settable<bool>(true),
                    Options = new Settable<List<Option>>(new List<Option> { new Option { Title = new Settable<string>("Option1") } }),
                    OptionsUnstructuredTemplate = new Settable<List<string>>(null)
                },
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST002"),
                    Version = new Settable<string>("2.0"),
                    CoreAndOptions = new Settable<bool>(true),
                    Options = new Settable<List<Option>>(null),
                    OptionsUnstructuredTemplate = new Settable<List<string>>(new List<string> { "Template1" })
                }
            };

            // Act
            var result = _sut.TestValidate(standards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_CoreAndOptions_Is_True_But_No_Options_Present()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST003"),
                    Version = new Settable<string>("3.0"),
                    CoreAndOptions = new Settable<bool>(true),
                    Options = new Settable<List<Option>>(null),
                    OptionsUnstructuredTemplate = new Settable<List<string>>(null)
                }
            };

            // Act
            var result = _sut.TestValidate(standards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "W1003: ST003 version 3.0 coreAndOptions is true, both options and optionsUnstructuredTemplate cannot be empty");
        }
    }
}
