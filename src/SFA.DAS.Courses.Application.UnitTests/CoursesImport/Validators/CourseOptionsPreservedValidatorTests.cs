using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class CourseOptionsPreservedValidatorTests
    {
        private CourseOptionsPreservedValidator _sut;
        private List<Domain.Entities.Standard> _currentStandards;

        [SetUp]
        public void SetUp()
        {
            _currentStandards = new List<Domain.Entities.Standard>
            {
                new Domain.Entities.Standard
                {
                    IfateReferenceNumber = "ST001",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    Options = new List<StandardOption>
                    {
                        StandardOption.Create("Option A"),
                        StandardOption.Create("Option B"),
                        StandardOption.Create("Option C")
                    }
                }
            };

            _sut = new CourseOptionsPreservedValidator(_currentStandards);
        }

        [Test]
        public void Should_Not_Add_Failure_When_All_Options_Are_Present()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    Options = new Settable<List<Option>>(new List<Option>
                    {
                        new Option { Title = new Settable<string>("Option A") },
                        new Option { Title = new Settable<string>("Option B") },
                        new Option { Title = new Settable<string>("Option C") }
                    })
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_One_Or_More_Options_Are_Removed()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    Options = new Settable<List<Option>>(new List<Option>
                    {
                        new Option { Title = new Settable<string>("Option A") }
                    })
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1014: ST001 version 1.0 has removed options: Option B, Option C");
        }

        [Test]
        public void Should_Add_Failure_When_One_Or_More_Options_Are_Changed()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    Options = new Settable<List<Option>>(new List<Option>
                    {
                        new Option { Title = new Settable<string>("Option X") },
                        new Option { Title = new Settable<string>("Option Y") }
                    })
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1013: ST001 version 1.0 has changed option titles: Option A → Option X; Option B → Option Y");
        }

        [Test]
        public void Should_Not_Add_Failure_When_An_Option_Is_Added()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST001"),
                    Version = new Settable<string>("1.0"),
                    Options = new Settable<List<Option>>(new List<Option>
                    {
                        new Option { Title = new Settable<string>("Option A") },
                        new Option { Title = new Settable<string>("Option B") },
                        new Option { Title = new Settable<string>("Option C") },
                        new Option { Title = new Settable<string>("Option D") }
                    })
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
