using System;
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
    public class StartDatesValidatorTests
    {
        private StartDatesValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new StartDatesValidator();
        }

        [Test]
        public void Should_Add_Failure_When_EarliestStartDate_Is_Invalid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    VersionEarliestStartDate = Settable<DateTime?>.FromInvalidValue("Invalid Date"),
                    VersionLatestStartDate = new Settable<DateTime?>(null)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1010: ST1001 version 1.0 the earliestStartDate 'Invalid Date' is not a valid date");
        }

        [Test]
        public void Should_Add_Failure_When_LatestStartDate_Is_Invalid()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1002"),
                    Version = new Settable<string>("1.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(null),
                    VersionLatestStartDate = Settable<DateTime?>.FromInvalidValue("Invalid Date")
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "S1011: ST1002 version 1.0 the latestStartDate 'Invalid Date' is not a valid date");
        }

        [Test]
        public void Should_Add_Failure_When_Dates_Are_Not_Contiguous_Across_Three_Standards()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(null),
                    VersionLatestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                },
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("2.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc)),
                    VersionLatestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc))
                },
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("3.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc)),
                    VersionLatestStartDate = new Settable<DateTime?>(null)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage ==
                "S1004: ST1001 version 1.0 and 2.0 do not have a contiguous latestStartDate and earliestStartDate");
            result.Errors.Should().Contain(error => error.ErrorMessage ==
                "S1004: ST1001 version 2.0 and 3.0 do not have a contiguous latestStartDate and earliestStartDate");
        }

        [Test]
        public void Should_Not_Add_Failure_When_Dates_Are_Valid_And_Contiguous_Across_Three_Standards()
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("1.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(null),
                    VersionLatestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                },
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("2.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)),
                    VersionLatestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc))
                },
                new Standard
                {
                    ReferenceNumber = new Settable<string>("ST1001"),
                    Version = new Settable<string>("3.0"),
                    VersionEarliestStartDate = new Settable<DateTime?>(new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc)),
                    VersionLatestStartDate = new Settable<DateTime?>(null)
                }
            };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
