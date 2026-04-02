using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests
{
    [TestFixture]
    public class RequiredFieldsForApprenticeshipUnitPresentValidatorTests
    {
        private RequiredFieldsForApprenticeshipUnitPresentValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new RequiredFieldsForApprenticeshipUnitPresentValidator();
        }

        private static ApprenticeshipUnit CreateValidApprenticeshipUnit()
        {
            return new ApprenticeshipUnit
            {
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                Change = new Settable<string>("Some change"),
                ChangedDate = new Settable<DateTime?>(null),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                Keywords = new Settable<List<string>>(new List<string>()),
                LastUpdated = new Settable<DateTime?>(null),
                LearningAimClassCode = new Settable<string>("ZSC12345"),
                Level = new Settable<int>(5),
                MinimumHoursForCompliance = new Settable<int>(1),
                ProposedMaxFunding = new Settable<int>(5000),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                ReferenceNumber = new Settable<string>("ST1001"),
                Regulated = new Settable<bool>(false),
                RegulatedBody = new Settable<string>("Regulator"),
                RegulationDetails = new Settable<List<ApprenticeshipUnit.RegulationDetail>>(new List<ApprenticeshipUnit.RegulationDetail>()),
                Route = new Settable<string>("Engineering"),
                TechnicalKnowledges = new Settable<List<ApprenticeshipUnit.IdDetailPair>>(new List<ApprenticeshipUnit.IdDetailPair>()),
                TechnicalSkills = new Settable<List<ApprenticeshipUnit.IdDetailPair>>(new List<ApprenticeshipUnit.IdDetailPair>()),
                Status = new Settable<string>("Approved for delivery"),
                Title = new Settable<string>("Title"),
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                ApprenticeshipUnitUrl = new Settable<Uri>(new Uri("http://standard.com")),
                Version = new Settable<string>("1.0"),
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionNumber = new Settable<string>("1.0"),
                WhoIsItFor = new Settable<string>("WhoIsItFor"),
            };
        }

        [Test]
        public void Should_Not_Add_Failure_When_All_Required_Fields_Are_Present()
        {
            // Arrange
            var importedStandard = CreateValidApprenticeshipUnit();
            var importedStandards = new List<ApprenticeshipUnit> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(nameof(ApprenticeshipUnit.ApprovedForDelivery))]
        [TestCase(nameof(ApprenticeshipUnit.CreatedDate))]
        [TestCase(nameof(ApprenticeshipUnit.Keywords))]
        [TestCase(nameof(ApprenticeshipUnit.TechnicalKnowledges))]
        [TestCase(nameof(ApprenticeshipUnit.LastUpdated))]
        [TestCase(nameof(ApprenticeshipUnit.LearningAimClassCode))]
        [TestCase(nameof(ApprenticeshipUnit.MinimumHoursForCompliance))]
        [TestCase(nameof(ApprenticeshipUnit.Level))]
        [TestCase(nameof(ApprenticeshipUnit.ProposedMaxFunding))]
        [TestCase(nameof(ApprenticeshipUnit.PublishDate))]
        [TestCase(nameof(ApprenticeshipUnit.ReferenceNumber))]
        [TestCase(nameof(ApprenticeshipUnit.Regulated))]
        [TestCase(nameof(ApprenticeshipUnit.RegulatedBody))]
        [TestCase(nameof(ApprenticeshipUnit.RegulationDetails))]
        [TestCase(nameof(ApprenticeshipUnit.Route))]
        [TestCase(nameof(ApprenticeshipUnit.TechnicalSkills))]
        [TestCase(nameof(ApprenticeshipUnit.Status))]
        [TestCase(nameof(ApprenticeshipUnit.Title))]
        [TestCase(nameof(ApprenticeshipUnit.TypicalJobTitles))]
        [TestCase(nameof(ApprenticeshipUnit.ApprenticeshipUnitUrl))]
        [TestCase(nameof(ApprenticeshipUnit.VersionEarliestStartDate))]
        [TestCase(nameof(ApprenticeshipUnit.VersionLatestEndDate))]
        [TestCase(nameof(ApprenticeshipUnit.VersionLatestStartDate))]
        [TestCase(nameof(ApprenticeshipUnit.Version))]
        [TestCase(nameof(ApprenticeshipUnit.WhoIsItFor))]
        public void Should_Add_Failure_When_Required_Field_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit>(propertyName);

            var importedStandard = CreateValidApprenticeshipUnit();
            typeof(ApprenticeshipUnit).GetProperty(propertyName)?.SetValue(importedStandard, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(ApprenticeshipUnit).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<ApprenticeshipUnit> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            if (propertyName == nameof(ApprenticeshipUnit.ReferenceNumber))
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: UNKNOWN version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
            }
            else if (propertyName == nameof(ApprenticeshipUnit.Version))
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version UNKNOWN has missing fields '{jsonPropertyName}'"));
            }
            else
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
            }
        }

        [TestCase(nameof(ApprenticeshipUnit.IdDetailPair.Id))]
        [TestCase(nameof(ApprenticeshipUnit.IdDetailPair.Detail))]
        public void Should_Add_Failure_When_Skill_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit.IdDetailPair>(propertyName);
            var skillsJsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit>(nameof(ApprenticeshipUnit.TechnicalSkills));

            var importedStandard = CreateValidApprenticeshipUnit();
            importedStandard.TechnicalSkills.Value.Add(new ApprenticeshipUnit.IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(ApprenticeshipUnit.IdDetailPair).GetProperty(propertyName)?.SetValue(importedStandard.TechnicalSkills.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(ApprenticeshipUnit.IdDetailPair).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<ApprenticeshipUnit> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{skillsJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(ApprenticeshipUnit.IdDetailPair.Id))]
        [TestCase(nameof(ApprenticeshipUnit.IdDetailPair.Detail))]
        public void Should_Add_Failure_When_Knowledge_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit.IdDetailPair>(propertyName);
            var knowledgesJsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit>(nameof(ApprenticeshipUnit.TechnicalKnowledges));

            var importedStandard = CreateValidApprenticeshipUnit();
            importedStandard.TechnicalKnowledges.Value.Add(new ApprenticeshipUnit.IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(ApprenticeshipUnit.IdDetailPair).GetProperty(propertyName)?.SetValue(importedStandard.TechnicalKnowledges.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(ApprenticeshipUnit.IdDetailPair).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<ApprenticeshipUnit> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{knowledgesJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(ApprenticeshipUnit.RegulationDetail.Name))]
        [TestCase(nameof(ApprenticeshipUnit.RegulationDetail.Approved))]
        public void Should_Add_Failure_When_RegulationDetail_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit.RegulationDetail>(propertyName);
            var regulationDetailJsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipUnit>(nameof(ApprenticeshipUnit.RegulationDetails));

            var importedStandard = CreateValidApprenticeshipUnit();
            importedStandard.RegulationDetails.Value.Add(new ApprenticeshipUnit.RegulationDetail { Name = new Settable<string>("Name"), Approved = new Settable<bool>(true), WebLink = string.Empty });
            typeof(ApprenticeshipUnit.RegulationDetail).GetProperty(propertyName)?.SetValue(importedStandard.RegulationDetails.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(ApprenticeshipUnit.RegulationDetail).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<ApprenticeshipUnit> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{regulationDetailJsonPropertyName}[0].{jsonPropertyName}'"));
        }
    }
}
