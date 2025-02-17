using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators
{
    [TestFixture]
    public class RequiredFieldsPresentValidatorTests
    {
        private RequiredFieldsPresentValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new RequiredFieldsPresentValidator();
        }

        private Standard CreateValidStandard()
        {
            return new Standard
            {
                ReferenceNumber = new Settable<string>("ST1001"),
                Version = new Settable<string>("1.0"),
                ApprovedForDelivery = new Settable<System.DateTime?>(System.DateTime.UtcNow),
                AssessmentPlanUrl = new Settable<string>("http://example.com"),
                Behaviours = new Settable<List<Behaviour>>(new List<Behaviour>()),
                Change = new Settable<string>("Some change"),
                CoreAndOptions = new Settable<bool>(true),
                CoronationEmblem = new Settable<bool>(false),
                CreatedDate = new Settable<System.DateTime>(System.DateTime.UtcNow),
                Duties = new Settable<List<Duty>>(new List<Duty>()),
                VersionEarliestStartDate = new Settable<System.DateTime?>(System.DateTime.UtcNow),
                EqaProvider = new Settable<EqaProvider>(new EqaProvider
                {
                    ContactAddress = new Settable<string>("Address"),
                    ContactEmail = new Settable<string>("email@example.com"),
                    ContactName = new Settable<string>("Contact Name"),
                    ProviderName = new Settable<string>("Provider"),
                    WebLink = new Settable<string>("http://provider.com")
                }),
                Keywords = new Settable<List<string>>(new List<string>()),
                Knowledges = new Settable<List<Knowledge>>(new List<Knowledge>()),
                VersionLatestEndDate = new Settable<System.DateTime?>(System.DateTime.UtcNow),
                VersionLatestStartDate = new Settable<System.DateTime?>(System.DateTime.UtcNow),
                LarsCode = new Settable<int>(12345),
                Level = new Settable<int>(5),
                ProposedMaxFunding = new Settable<int>(5000),
                Options = new Settable<List<Option>>(new List<Option>()),
                OptionsUnstructuredTemplate = new Settable<List<string>>(new List<string>()),
                OverviewOfRole = new Settable<string>("Overview"),
                PublishDate = new Settable<System.DateTime>(System.DateTime.UtcNow),
                RegulatedBody = new Settable<string>("Regulator"),
                Route = new Settable<string>("Engineering"),
                Skills = new Settable<List<Skill>>(new List<Skill>()),
                StandardPageUrl = new Settable<System.Uri>(new System.Uri("http://standard.com")),
                Status = new Settable<string>("Approved for delivery"),
                TbMainContact = new Settable<string>("Main Contact"),
                Title = new Settable<string>("Title"),
                ProposedTypicalDuration = new Settable<int>(12),
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                VersionNumber = new Settable<string>("1.0")
            };
        }

        [Test]
        public void Should_Not_Add_Failure_When_All_Required_Fields_Are_Present()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase("referenceNumber", "ReferenceNumber")]
        [TestCase("version", "Version")]
        [TestCase("approvedForDelivery", "ApprovedForDelivery")]
        [TestCase("assessmentPlanUrl", "AssessmentPlanUrl")]
        [TestCase("behaviours", "Behaviours")]
        [TestCase("change", "Change")]
        [TestCase("coreAndOptions", "CoreAndOptions")]
        [TestCase("coronationEmblem", "CoronationEmblem")]
        [TestCase("createdDate", "CreatedDate")]
        [TestCase("duties", "Duties")]
        [TestCase("earliestStartDate", "VersionEarliestStartDate")]
        [TestCase("keywords", "Keywords")]
        [TestCase("knowledges", "Knowledges")]
        [TestCase("latestEndDate", "VersionLatestEndDate")]
        [TestCase("latestStartDate", "VersionLatestStartDate")]
        [TestCase("larsCode", "LarsCode")]
        [TestCase("level", "Level")]
        [TestCase("maxFunding", "ProposedMaxFunding")]
        [TestCase("overviewOfRole", "OverviewOfRole")]
        [TestCase("publishDate", "PublishDate")]
        [TestCase("regulatedBody", "RegulatedBody")]
        [TestCase("route", "Route")]
        [TestCase("skills", "Skills")]
        [TestCase("standardPageUrl", "StandardPageUrl")]
        [TestCase("status", "Status")]
        [TestCase("tbMainContact", "TbMainContact")]
        [TestCase("title", "Title")]
        [TestCase("typicalDuration", "ProposedTypicalDuration")]
        [TestCase("typicalJobTitles", "TypicalJobTitles")]
        [TestCase("versionNumber", "VersionNumber")]
        public void Should_Add_Failure_When_Required_Field_Is_Missing(string jsonPropertyName, string propertyName)
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            typeof(Standard).GetProperty(propertyName)?.SetValue(importedStandard, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Standard).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            if (propertyName == nameof(Standard.ReferenceNumber))
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: UNKNOWN version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
            }
            else if (propertyName == nameof(Standard.Version))
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version UNKNOWN has missing fields '{jsonPropertyName}'"));
            }
            else
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
            }
        }

        [Test]
        public void Should_Add_Failure_When_Options_And_OptionsUnstructuredTemplate_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.Options = new Settable<List<Option>>();
            importedStandard.OptionsUnstructuredTemplate = new Settable<List<string>>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'options or optionsUnstructuredTemplate'"));
        }

        [Test]
        public void Should_Not_Add_Failure_When_Options_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.Options = new Settable<List<Option>>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Not_Add_Failure_When_OptionsUnstructuredTemplate_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.OptionsUnstructuredTemplate = new Settable<List<string>>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider = new Settable<EqaProvider>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider'"));
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_ContactAddress_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider.Value.ContactAddress = new Settable<string>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider.contactAddress'"));
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_ContactEmail_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider.Value.ContactEmail = new Settable<string>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider.contactEmail'"));
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_ContactName_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider.Value.ContactName = new Settable<string>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider.contactName'"));
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_ProviderName_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider.Value.ProviderName = new Settable<string>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider.providerName'"));
        }

        [Test]
        public void Should_Add_Failure_When_EqaProvider_WebLink_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidStandard();
            importedStandard.EqaProvider.Value.WebLink = new Settable<string>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields 'eQAProvider.webLink'"));
        }
    }
}
