using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class LarsCodeNotDuplicatedValidatorTests
    {
        private LarsCodeNotDuplicatedValidator _sut;
        private Dictionary<string, List<StandardImport>> _allStandardImports;

        [SetUp]
        public void SetUp()
        {
            _allStandardImports = new Dictionary<string, List<StandardImport>>
            {
                {
                    "ST1001", new List<StandardImport>
                    {
                        new StandardImport { IfateReferenceNumber = "ST1001", Version = "1.0", LarsCode = 12345 }
                    }
                },
                {
                    "ST1002", new List<StandardImport>
                    {
                        new StandardImport { IfateReferenceNumber = "ST1002", Version = "1.0", LarsCode = 67890 }
                    }
                }
            };

            _sut = new LarsCodeNotDuplicatedValidator(_allStandardImports);
        }

        [Test]
        public void Should_Not_Add_Failure_When_LarsCode_Is_Unique()
        {
            // Arrange
            var standardImports = new List<StandardImport>
            {
                new StandardImport { IfateReferenceNumber = "ST1003", Version = "1.0", LarsCode = 11111 }
            };

            // Act
            var result = _sut.TestValidate(standardImports);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_LarsCode_Is_Duplicated()
        {
            // Arrange
            var standardImports = new List<StandardImport>
            {
                new StandardImport { IfateReferenceNumber = "ST1003", Version = "1.0", LarsCode = 12345 }
            };

            // Act
            var result = _sut.TestValidate(standardImports);

            // Assert
            result.Errors.Should().ContainSingle(error => error.ErrorMessage ==
                "E1004: ST1003 version 1.0 has duplicated larsCode 12345 with standard ST1001 version 1.0");
        }
    }
}
