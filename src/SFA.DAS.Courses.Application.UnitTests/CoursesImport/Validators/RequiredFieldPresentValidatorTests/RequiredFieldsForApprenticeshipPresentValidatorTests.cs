using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using static SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Apprenticeship;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests
{
    [TestFixture]
    public class RequiredFieldsForApprenticeshipPresentValidatorTests
    {
        private RequiredFieldsForApprenticeshipPresentValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new RequiredFieldsForApprenticeshipPresentValidator();
        }

        private static Apprenticeship CreateValidApprenticeship()
        {
            return new Apprenticeship
            {
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                AssessmentPlanUrl = new Settable<string>("http://example.com"),
                Behaviours = new Settable<List<Apprenticeship.Behaviour>>(new List<Apprenticeship.Behaviour>()),
                Change = new Settable<string>("Some change"),
                ChangedDate = new Settable<DateTime?>(null),
                CoreAndOptions = new Settable<bool>(true),
                CoronationEmblem = new Settable<bool>(false),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                Duties = new Settable<List<Apprenticeship.Duty>>(new List<Apprenticeship.Duty>()),
                EqaProvider = new Settable<Apprenticeship.ApprenticeshipEqaProvider>(new Apprenticeship.ApprenticeshipEqaProvider
                {
                    ContactAddress = new Settable<string>("Address"),
                    ContactEmail = new Settable<string>("email@example.com"),
                    ContactName = new Settable<string>("Contact Name"),
                    ProviderName = new Settable<string>("Provider"),
                    WebLink = new Settable<string>("http://provider.com")
                }),
                IntegratedApprenticeship = new Settable<bool?>(null),
                IntegratedDegree = new Settable<string>(),
                Keywords = new Settable<List<string>>(new List<string>()),
                Knowledges = new Settable<List<Apprenticeship.Knowledge>>(new List<Apprenticeship.Knowledge>()),
                LarsCode = new Settable<int>(12345),
                LastUpdated = new Settable<DateTime?>(null),
                Level = new Settable<int>(5),
                Options = new Settable<List<Apprenticeship.Option>>(new List<Apprenticeship.Option>()),
                OptionsUnstructuredTemplate = new Settable<List<string>>(new List<string>()),
                OverviewOfRole = new Settable<string>("Overview"),
                ProposedMaxFunding = new Settable<int>(5000),
                ProposedTypicalDuration = new Settable<int>(12),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                ReferenceNumber = new Settable<string>("ST1001"),
                Regulated = new Settable<bool>(false),
                RegulatedBody = new Settable<string>("Regulator"),
                RegulationDetails = new Settable<List<Apprenticeship.RegulationDetail>>(new List<Apprenticeship.RegulationDetail>()),
                Route = new Settable<string>("Engineering"),
                RouteCode = new Settable<int>(0),
                Skills = new Settable<List<Apprenticeship.Skill>>(new List<Apprenticeship.Skill>()),
                StandardPageUrl = new Settable<Uri>(new Uri("http://standard.com")),
                Status = new Settable<string>("Approved for delivery"),
                TbMainContact = new Settable<string>("Main Contact"),
                Title = new Settable<string>("Title"),
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                Version = new Settable<string>("1.0"),
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionNumber = new Settable<string>("1.0"),
            };
        }

        [Test]
        public void Should_Not_Add_Failure_When_All_Required_Fields_Are_Present()
        {
            // Arrange
            var importedStandard = CreateValidApprenticeship();
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }


        [TestCase(nameof(Apprenticeship.ApprovedForDelivery))]
        [TestCase(nameof(Apprenticeship.AssessmentPlanUrl))]
        [TestCase(nameof(Apprenticeship.Behaviours))]
        [TestCase(nameof(Apprenticeship.Change))]
        [TestCase(nameof(Apprenticeship.CoreAndOptions))]
        [TestCase(nameof(Apprenticeship.CoronationEmblem))]
        [TestCase(nameof(Apprenticeship.CreatedDate))]
        [TestCase(nameof(Apprenticeship.Duties))]
        [TestCase(nameof(Apprenticeship.EqaProvider))]
        [TestCase(nameof(Apprenticeship.Keywords))]
        [TestCase(nameof(Apprenticeship.Knowledges))]
        [TestCase(nameof(Apprenticeship.LarsCode))]
        [TestCase(nameof(Apprenticeship.Level))]
        [TestCase(nameof(Apprenticeship.ProposedMaxFunding))]
        [TestCase(nameof(Apprenticeship.OverviewOfRole))]
        [TestCase(nameof(Apprenticeship.PublishDate))]
        [TestCase(nameof(Apprenticeship.ReferenceNumber))]
        [TestCase(nameof(Apprenticeship.RegulatedBody))]
        [TestCase(nameof(Apprenticeship.Regulated))]
        [TestCase(nameof(Apprenticeship.RegulationDetails))]
        [TestCase(nameof(Apprenticeship.Route))]
        [TestCase(nameof(Apprenticeship.Skills))]
        [TestCase(nameof(Apprenticeship.StandardPageUrl))]
        [TestCase(nameof(Apprenticeship.Status))]
        [TestCase(nameof(Apprenticeship.TbMainContact))]
        [TestCase(nameof(Apprenticeship.Title))]
        [TestCase(nameof(Apprenticeship.ProposedTypicalDuration))]
        [TestCase(nameof(Apprenticeship.TypicalJobTitles))]
        [TestCase(nameof(Apprenticeship.VersionEarliestStartDate))]
        [TestCase(nameof(Apprenticeship.VersionLatestEndDate))]
        [TestCase(nameof(Apprenticeship.VersionLatestStartDate))]
        [TestCase(nameof(Apprenticeship.VersionNumber))]
        [TestCase(nameof(Apprenticeship.Version))]
        public void Should_Add_Failure_When_Required_Field_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(propertyName);

            var importedStandard = CreateValidApprenticeship();
            typeof(Apprenticeship).GetProperty(propertyName)?.SetValue(importedStandard, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            if (propertyName == nameof(Apprenticeship.ReferenceNumber))
            {
                result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: UNKNOWN version {importedStandard.Version} has missing fields '{jsonPropertyName}'"));
            }
            else if (propertyName == nameof(Apprenticeship.Version))
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
            var optionsJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Options));
            var optionsUnstructuredTemplateJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.OptionsUnstructuredTemplate));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Options = new Settable<List<Apprenticeship.Option>>();
            importedStandard.OptionsUnstructuredTemplate = new Settable<List<string>>();
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{optionsJsonPropertyName} or {optionsUnstructuredTemplateJsonPropertyName}'"));
        }

        [Test]
        public void Should_Not_Add_Failure_When_Options_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidApprenticeship();
            importedStandard.Options = new Settable<List<Apprenticeship.Option>>();
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Not_Add_Failure_When_OptionsUnstructuredTemplate_Is_Missing()
        {
            // Arrange
            var importedStandard = CreateValidApprenticeship();
            importedStandard.OptionsUnstructuredTemplate = new Settable<List<string>>();
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(nameof(ApprenticeshipEqaProvider.ContactAddress))]
        [TestCase(nameof(ApprenticeshipEqaProvider.ContactEmail))]
        [TestCase(nameof(ApprenticeshipEqaProvider.ContactName))]
        [TestCase(nameof(ApprenticeshipEqaProvider.ProviderName))]
        [TestCase(nameof(ApprenticeshipEqaProvider.WebLink))]
        public void Should_Add_Failure_When_EqaProvider_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<ApprenticeshipEqaProvider>(propertyName);
            var eQAProviderJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.EqaProvider));

            var importedStandard = CreateValidApprenticeship();
            typeof(ApprenticeshipEqaProvider).GetProperty(propertyName)?.SetValue(importedStandard.EqaProvider.Value, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(ApprenticeshipEqaProvider).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{eQAProviderJsonPropertyName}.{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.Option.OptionId))]
        [TestCase(nameof(Apprenticeship.Option.Title))]
        public void Should_Add_Failure_When_Option_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.Option>(propertyName);
            var optionsJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Options));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Options.Value.Add(new Apprenticeship.Option { OptionId = new Settable<Guid>(Guid.NewGuid()), Title = new Settable<string>("Title") });
            typeof(Apprenticeship.Option).GetProperty(propertyName)?.SetValue(importedStandard.Options.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.Option).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{optionsJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.Skill.SkillId))]
        [TestCase(nameof(Apprenticeship.Skill.Detail))]
        public void Should_Add_Failure_When_Skill_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.Skill>(propertyName);
            var skillsJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Skills));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Skills.Value.Add(new Apprenticeship.Skill { SkillId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Apprenticeship.Skill).GetProperty(propertyName)?.SetValue(importedStandard.Skills.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.Skill).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{skillsJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.Duty.DutyId))]
        [TestCase(nameof(Apprenticeship.Duty.DutyDetail))]
        [TestCase(nameof(Apprenticeship.Duty.IsThisACoreDuty))]
        [TestCase(nameof(Apprenticeship.Duty.MappedKnowledge))]
        [TestCase(nameof(Apprenticeship.Duty.MappedBehaviour))]
        [TestCase(nameof(Apprenticeship.Duty.MappedOptions))]
        [TestCase(nameof(Apprenticeship.Duty.MappedSkills))]
        public void Should_Add_Failure_When_Duty_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.Duty>(propertyName);
            var dutiesJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Duties));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Duties.Value.Add(new Apprenticeship.Duty
            {
                DutyId = new Settable<Guid>(Guid.NewGuid()),
                DutyDetail = new Settable<string>("DutyDetail"),
                IsThisACoreDuty = new Settable<long>(0),
                MappedKnowledge = new Settable<List<Guid>>(new List<Guid>()),
                MappedBehaviour = new Settable<List<Guid>>(new List<Guid>()),
                MappedOptions = new Settable<List<Guid>>(new List<Guid>()),
                MappedSkills = new Settable<List<Guid>>(new List<Guid>())
            });
            typeof(Apprenticeship.Duty).GetProperty(propertyName)?.SetValue(importedStandard.Duties.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.Duty).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{dutiesJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.Behaviour.BehaviourId))]
        [TestCase(nameof(Apprenticeship.Behaviour.Detail))]
        public void Should_Add_Failure_When_Behaviour_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.Behaviour>(propertyName);
            var behavioursJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Behaviours));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Behaviours.Value.Add(new Apprenticeship.Behaviour { BehaviourId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Apprenticeship.Behaviour).GetProperty(propertyName)?.SetValue(importedStandard.Behaviours.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.Behaviour).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{behavioursJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.Knowledge.KnowledgeId))]
        [TestCase(nameof(Apprenticeship.Knowledge.Detail))]
        public void Should_Add_Failure_When_Knowledge_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.Knowledge>(propertyName);
            var knowledgesJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.Knowledges));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.Knowledges.Value.Add(new Apprenticeship.Knowledge { KnowledgeId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Apprenticeship.Knowledge).GetProperty(propertyName)?.SetValue(importedStandard.Knowledges.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.Knowledge).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{knowledgesJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Apprenticeship.RegulationDetail.Name))]
        [TestCase(nameof(Apprenticeship.RegulationDetail.Approved))]
        public void Should_Add_Failure_When_RegulationDetail_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship.RegulationDetail>(propertyName);
            var regulationDetailJsonPropertyName = JsonHelper.GetJsonPropertyName<Apprenticeship>(nameof(Apprenticeship.RegulationDetails));

            var importedStandard = CreateValidApprenticeship();
            importedStandard.RegulationDetails.Value.Add(new Apprenticeship.RegulationDetail { Name = new Settable<string>("Name"), Approved = new Settable<bool>(true), WebLink = string.Empty });
            typeof(Apprenticeship.RegulationDetail).GetProperty(propertyName)?.SetValue(importedStandard.RegulationDetails.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Apprenticeship.RegulationDetail).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Apprenticeship> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{regulationDetailJsonPropertyName}[0].{jsonPropertyName}'"));
        }
    }
}
