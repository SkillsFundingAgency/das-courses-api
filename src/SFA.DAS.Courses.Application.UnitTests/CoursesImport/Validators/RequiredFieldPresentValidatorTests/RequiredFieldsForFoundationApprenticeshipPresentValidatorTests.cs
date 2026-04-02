using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests;

public class RequiredFieldsForFoundationApprenticeshipPresentValidatorTests
{
    private RequiredFieldsForFoundationApprenticeshipPresentValidator _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new RequiredFieldsForFoundationApprenticeshipPresentValidator();
    }

    private static FoundationApprenticeship CreateValidFoundationApprenticeship()
        => new()
        {
            ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
            AssessmentChanged = new Settable<bool>(false),
            AssessmentPlanUrl = new Settable<string>("http://example.com"),
            Change = new Settable<string>("Some change"),
            ChangedDate = new Settable<DateTime?>(null),
            CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
            EmployabilitySkillsAndBehaviours = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
            EqaProvider = new Settable<FoundationApprenticeship.FoundationApprenticeshipEqaProvider>(new FoundationApprenticeship.FoundationApprenticeshipEqaProvider
            {
                ContactAddress = new Settable<string>("Address"),
                ContactEmail = new Settable<string>("email@example.com"),
                ContactName = new Settable<string>("Contact Name"),
                ProviderName = new Settable<string>("Provider"),
                WebLink = new Settable<string>("http://provider.com")
            }),
            FoundationApprenticeshipUrl = new Settable<Uri>(new Uri("http://foundation.com")),
            Keywords = new Settable<List<string>>(new List<string>()),
            LarsCode = new Settable<int>(12345),
            LastUpdated = new Settable<DateTime?>(null),
            Level = new Settable<int>(5),
            OverviewOfRole = new Settable<string>("Overview"),
            ProposedMaxFunding = new Settable<int>(5000),
            ProposedTypicalDuration = new Settable<int>(12),
            PublishDate = new Settable<DateTime>(DateTime.UtcNow),
            ReferenceNumber = new Settable<string>("FA0001"),
            Regulated = new Settable<bool>(false),
            RegulatedBody = new Settable<string>("Regulator"),
            RegulationDetails = new Settable<List<FoundationApprenticeship.RegulationDetail>>(new List<FoundationApprenticeship.RegulationDetail>()),
            RelatedOccupations = new Settable<List<FoundationApprenticeship.RelatedOccupation>>(new List<FoundationApprenticeship.RelatedOccupation>()),
            Route = new Settable<string>("Engineering"),
            RouteCode = new Settable<int>(0),
            Status = new Settable<string>("Approved for delivery"),
            TechnicalKnowledges = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
            TechnicalSkills = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
            Title = new Settable<string>("Title"),
            TypicalJobTitles = new Settable<List<string>>(new List<string>()),
            Version = new Settable<string>("1.0"),
            VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
            VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            VersionNumber = new Settable<string>("1.0"),

        };

    [Test]
    public void Should_Not_Add_Failure_When_All_Required_Fields_Are_Present()
    {
        RequiredFieldsForFoundationApprenticeshipPresentValidator sut = new();
        // Arrange
        var importedStandard = CreateValidFoundationApprenticeship();
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = sut.TestValidate(importedStandards);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(nameof(FoundationApprenticeship.ApprovedForDelivery))]
    [TestCase(nameof(FoundationApprenticeship.AssessmentPlanUrl))]
    [TestCase(nameof(FoundationApprenticeship.Change))]
    [TestCase(nameof(FoundationApprenticeship.CreatedDate))]
    [TestCase(nameof(FoundationApprenticeship.EqaProvider))]
    [TestCase(nameof(FoundationApprenticeship.Keywords))]
    [TestCase(nameof(FoundationApprenticeship.LarsCode))]
    [TestCase(nameof(FoundationApprenticeship.Level))]
    [TestCase(nameof(FoundationApprenticeship.ProposedMaxFunding))]
    [TestCase(nameof(FoundationApprenticeship.OverviewOfRole))]
    [TestCase(nameof(FoundationApprenticeship.PublishDate))]
    [TestCase(nameof(FoundationApprenticeship.ReferenceNumber))]
    [TestCase(nameof(FoundationApprenticeship.RegulatedBody))]
    [TestCase(nameof(FoundationApprenticeship.Regulated))]
    [TestCase(nameof(FoundationApprenticeship.RegulationDetails))]
    [TestCase(nameof(FoundationApprenticeship.Route))]
    [TestCase(nameof(FoundationApprenticeship.Status))]
    [TestCase(nameof(FoundationApprenticeship.Title))]
    [TestCase(nameof(FoundationApprenticeship.ProposedTypicalDuration))]
    [TestCase(nameof(FoundationApprenticeship.TypicalJobTitles))]
    [TestCase(nameof(FoundationApprenticeship.VersionEarliestStartDate))]
    [TestCase(nameof(FoundationApprenticeship.VersionLatestEndDate))]
    [TestCase(nameof(FoundationApprenticeship.VersionLatestStartDate))]
    [TestCase(nameof(FoundationApprenticeship.VersionNumber))]
    [TestCase(nameof(FoundationApprenticeship.Version))]
    [TestCase(nameof(FoundationApprenticeship.TechnicalKnowledges))]
    [TestCase(nameof(FoundationApprenticeship.TechnicalSkills))]
    [TestCase(nameof(FoundationApprenticeship.EmployabilitySkillsAndBehaviours))]
    [TestCase(nameof(FoundationApprenticeship.FoundationApprenticeshipUrl))]
    [TestCase(nameof(FoundationApprenticeship.AssessmentChanged))]
    [TestCase(nameof(FoundationApprenticeship.RelatedOccupations))]
    public void Should_Add_Failure_When_Required_Field_Is_Missing(string propertyName)
    {
        // Arrange
        var jsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship>(propertyName);

        var importedStandard = CreateValidFoundationApprenticeship();
        typeof(FoundationApprenticeship).GetProperty(propertyName)?.SetValue(importedStandard, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(FoundationApprenticeship).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = _sut.TestValidate(importedStandards);

        // Assert
        if (propertyName == nameof(FoundationApprenticeship.ReferenceNumber))
        {
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: UNKNOWN version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
        }
        else if (propertyName == nameof(FoundationApprenticeship.Version))
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
        // Arrange
        var childJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship.IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship>(nameof(FoundationApprenticeship.TechnicalSkills));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.TechnicalSkills.Value.Add(new FoundationApprenticeship.IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.TechnicalSkills.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = _sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(IdDetailPair.Id))]
    [TestCase(nameof(IdDetailPair.Detail))]
    public void Should_Add_Failure_When_TechnicalKnowledges_Child_Property_Is_Missing(string childPropertyName)
    {
        // Arrange
        var childJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship.IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship>(nameof(FoundationApprenticeship.TechnicalKnowledges));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.TechnicalKnowledges.Value.Add(new FoundationApprenticeship.IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.TechnicalKnowledges.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = _sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(IdDetailPair.Id))]
    [TestCase(nameof(IdDetailPair.Detail))]
    public void Should_Add_Failure_When_EmployabilitySkillsAndBehaviours_Child_Property_Is_Missing(string childPropertyName)
    {
        // Arrange
        var childJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship.IdDetailPair>(childPropertyName);
        var rootJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship>(nameof(FoundationApprenticeship.EmployabilitySkillsAndBehaviours));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.EmployabilitySkillsAndBehaviours.Value.Add(new FoundationApprenticeship.IdDetailPair { Id = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
        typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName)?.SetValue(importedStandard.EmployabilitySkillsAndBehaviours.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(FoundationApprenticeship.IdDetailPair).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = _sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }

    [TestCase(nameof(FoundationApprenticeship.RelatedOccupation.Name))]
    [TestCase(nameof(FoundationApprenticeship.RelatedOccupation.Reference))]
    public void Should_Add_Failure_When_RelatedOccupations_Child_Property_Is_Missing(string childPropertyName)
    {
        // Arrange
        var childJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship.RelatedOccupation>(childPropertyName);
        var rootJsonPropertyName = JsonHelper.GetJsonPropertyName<FoundationApprenticeship>(nameof(FoundationApprenticeship.RelatedOccupations));

        var importedStandard = CreateValidFoundationApprenticeship();
        importedStandard.RelatedOccupations.Value.Add(new FoundationApprenticeship.RelatedOccupation { Name = new Settable<string>(Guid.NewGuid().ToString()), Reference = new Settable<string>("OCC1001") });
        typeof(FoundationApprenticeship.RelatedOccupation).GetProperty(childPropertyName)?.SetValue(importedStandard.RelatedOccupations.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(FoundationApprenticeship.RelatedOccupation).GetProperty(childPropertyName).PropertyType.GenericTypeArguments[0])));
        var importedStandards = new List<FoundationApprenticeship> { importedStandard };

        // Act
        var result = _sut.TestValidate(importedStandards);

        // Assert
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{rootJsonPropertyName}[0].{childJsonPropertyName}'"));
    }
}
