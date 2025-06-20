using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests;

public class FoundationTypeRequiredFieldValidationTests
{
    private static Standard CreateValidFoundationApprenticeship()
        => new()
        {
            ApprenticeshipType = Domain.Entities.ApprenticeshipType.FoundationApprenticeship,
            ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
            AssessmentPlanUrl = new Settable<string>("http://example.com"),
            Change = new Settable<string>("Some change"),
            CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
            EqaProvider = new Settable<EqaProvider>(new EqaProvider
            {
                ContactAddress = new Settable<string>("Address"),
                ContactEmail = new Settable<string>("email@example.com"),
                ContactName = new Settable<string>("Contact Name"),
                ProviderName = new Settable<string>("Provider"),
                WebLink = new Settable<string>("http://provider.com")
            }),
            VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            Keywords = new Settable<List<string>>(new List<string>()),
            LarsCode = new Settable<int>(12345),
            VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
            VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            Level = new Settable<int>(5),
            ProposedMaxFunding = new Settable<int>(5000),
            OverviewOfRole = new Settable<string>("Overview"),
            PublishDate = new Settable<DateTime>(DateTime.UtcNow),
            ReferenceNumber = new Settable<string>("FA0001"),
            RegulatedBody = new Settable<string>("Regulator"),
            Regulated = new Settable<bool>(false),
            RegulationDetail = new Settable<List<RegulationDetail>>(new List<RegulationDetail>()),
            Route = new Settable<string>("Engineering"),
            Status = new Settable<string>("Approved for delivery"),
            Title = new Settable<string>("Title"),
            ProposedTypicalDuration = new Settable<int>(12),
            TypicalJobTitles = new Settable<List<string>>(new List<string>()),
            Version = new Settable<string>("1.0"),
            VersionNumber = new Settable<string>("1.0"),
            FoundationApprenticeshipUrl = new Settable<Uri>(new Uri("http://foundation.com")),
            TechnicalSkills = new Settable<List<IdDetailPair>>(new List<IdDetailPair>()),
            TechnicalKnowledges = new Settable<List<IdDetailPair>>(new List<IdDetailPair>()),
            EmployabilitySkillsAndBehaviours = new Settable<List<IdDetailPair>>(new List<IdDetailPair>()),
            AssessmentChanged = new Settable<bool>(false),
            RelatedOccupations = new Settable<List<RelatedOccupation>>(new List<RelatedOccupation>())
        };

    [Test]
    public void Should_Not_Add_Failure_When_All_Required_Fields_Are_Present()
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var importedStandard = CreateValidFoundationApprenticeship();
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(nameof(Standard.ApprovedForDelivery))]
    [TestCase(nameof(Standard.AssessmentPlanUrl))]
    [TestCase(nameof(Standard.Change))]
    [TestCase(nameof(Standard.CreatedDate))]
    [TestCase(nameof(Standard.EqaProvider))]
    [TestCase(nameof(Standard.Keywords))]
    [TestCase(nameof(Standard.LarsCode))]
    [TestCase(nameof(Standard.Level))]
    [TestCase(nameof(Standard.ProposedMaxFunding))]
    [TestCase(nameof(Standard.OverviewOfRole))]
    [TestCase(nameof(Standard.PublishDate))]
    [TestCase(nameof(Standard.ReferenceNumber))]
    [TestCase(nameof(Standard.RegulatedBody))]
    [TestCase(nameof(Standard.Regulated))]
    [TestCase(nameof(Standard.RegulationDetail))]
    [TestCase(nameof(Standard.Route))]
    [TestCase(nameof(Standard.Status))]
    [TestCase(nameof(Standard.Title))]
    [TestCase(nameof(Standard.ProposedTypicalDuration))]
    [TestCase(nameof(Standard.TypicalJobTitles))]
    [TestCase(nameof(Standard.VersionEarliestStartDate))]
    [TestCase(nameof(Standard.VersionLatestEndDate))]
    [TestCase(nameof(Standard.VersionLatestStartDate))]
    [TestCase(nameof(Standard.VersionNumber))]
    [TestCase(nameof(Standard.Version))]
    [TestCase(nameof(Standard.TechnicalKnowledges))]
    [TestCase(nameof(Standard.TechnicalSkills))]
    [TestCase(nameof(Standard.EmployabilitySkillsAndBehaviours))]
    [TestCase(nameof(Standard.FoundationApprenticeshipUrl))]
    [TestCase(nameof(Standard.AssessmentChanged))]
    [TestCase(nameof(Standard.RelatedOccupations))]
    public void Should_Add_Failure_When_Required_Field_Is_Missing(string propertyName)
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var jsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<Standard>(propertyName);

        var importedStandard = CreateValidFoundationApprenticeship();
        typeof(Standard).GetProperty(propertyName)?.SetValue(importedStandard, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Standard).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

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

    [TestCase(nameof(IdDetailPair.Id))]
    [TestCase(nameof(IdDetailPair.Detail))]
    public void Should_Add_Failure_When_TechnicalSkills_Child_Property_Is_Missing(string childPropertyName)
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var childJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<Standard>(nameof(Standard.TechnicalSkills));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.TechnicalSkills.Value.Add(new IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.TechnicalSkills.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(IdDetailPair.Id))]
    [TestCase(nameof(IdDetailPair.Detail))]
    public void Should_Add_Failure_When_TechnicalKnowledges_Child_Property_Is_Missing(string childPropertyName)
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var childJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<Standard>(nameof(Standard.TechnicalKnowledges));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.TechnicalKnowledges.Value.Add(new IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.TechnicalKnowledges.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(IdDetailPair.Id))]
    [TestCase(nameof(IdDetailPair.Detail))]
    public void Should_Add_Failure_When_EmployabilitySkillsAndBehaviours_Child_Property_Is_Missing(string childPropertyName)
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var childJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<Standard>(nameof(Standard.EmployabilitySkillsAndBehaviours));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.EmployabilitySkillsAndBehaviours.Value.Add(new IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.EmployabilitySkillsAndBehaviours.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(RelatedOccupation.Name))]
    [TestCase(nameof(RelatedOccupation.Reference))]
    public void Should_Add_Failure_When_RelatedOccupations_Child_Property_Is_Missing(string childPropertyName)
    {
        RequiredFieldsPresentValidator sut = new();
        // Arrange
        var childJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<RelatedOccupation>(childPropertyName);
        var rootJsonPropertyName = RequiredFieldsPresentValidator.GetJsonPropertyName<Standard>(nameof(Standard.RelatedOccupations));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.RelatedOccupations.Value.Add(new RelatedOccupation { Name = new Settable<string>(Guid.NewGuid().ToString()), Reference = new Settable<string>("OCC1001") });
        typeof(RelatedOccupation).GetProperty(childPropertyName)?.SetValue(importedStandard.RelatedOccupations.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(RelatedOccupation).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<Standard> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }
}
