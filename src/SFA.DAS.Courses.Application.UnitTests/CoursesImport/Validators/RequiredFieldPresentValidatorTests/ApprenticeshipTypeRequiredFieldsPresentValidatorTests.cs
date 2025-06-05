using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation.TestHelper;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests
{
    [TestFixture]
    public class ApprenticeshipTypeRequiredFieldsPresentValidatorTests
    {
        private RequiredFieldsPresentValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new RequiredFieldsPresentValidator();
        }

        private static Standard CreateValidStandard()
        {
            return new Standard
            {
                ReferenceNumber = new Settable<string>("ST1001"),
                Version = new Settable<string>("1.0"),
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                AssessmentPlanUrl = new Settable<string>("http://example.com"),
                Behaviours = new Settable<List<Behaviour>>(new List<Behaviour>()),
                Change = new Settable<string>("Some change"),
                CoreAndOptions = new Settable<bool>(true),
                CoronationEmblem = new Settable<bool>(false),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                Duties = new Settable<List<Duty>>(new List<Duty>()),
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
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
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                LarsCode = new Settable<int>(12345),
                Level = new Settable<int>(5),
                ProposedMaxFunding = new Settable<int>(5000),
                Options = new Settable<List<Option>>(new List<Option>()),
                OptionsUnstructuredTemplate = new Settable<List<string>>(new List<string>()),
                OverviewOfRole = new Settable<string>("Overview"),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                RegulatedBody = new Settable<string>("Regulator"),
                Regulated = new Settable<bool>(false),
                RegulationDetail = new Settable<List<RegulationDetail>>(new List<RegulationDetail>()),
                Route = new Settable<string>("Engineering"),
                Skills = new Settable<List<Skill>>(new List<Skill>()),
                StandardPageUrl = new Settable<Uri>(new Uri("http://standard.com")),
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


        [TestCase(nameof(Standard.ApprovedForDelivery))]
        [TestCase(nameof(Standard.AssessmentPlanUrl))]
        [TestCase(nameof(Standard.Behaviours))]
        [TestCase(nameof(Standard.Change))]
        [TestCase(nameof(Standard.CoreAndOptions))]
        [TestCase(nameof(Standard.CoronationEmblem))]
        [TestCase(nameof(Standard.CreatedDate))]
        [TestCase(nameof(Standard.Duties))]
        [TestCase(nameof(Standard.EqaProvider))]
        [TestCase(nameof(Standard.Keywords))]
        [TestCase(nameof(Standard.Knowledges))]
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
        [TestCase(nameof(Standard.Skills))]
        [TestCase(nameof(Standard.StandardPageUrl))]
        [TestCase(nameof(Standard.Status))]
        [TestCase(nameof(Standard.TbMainContact))]
        [TestCase(nameof(Standard.Title))]
        [TestCase(nameof(Standard.ProposedTypicalDuration))]
        [TestCase(nameof(Standard.TypicalJobTitles))]
        [TestCase(nameof(Standard.VersionEarliestStartDate))]
        [TestCase(nameof(Standard.VersionLatestEndDate))]
        [TestCase(nameof(Standard.VersionLatestStartDate))]
        [TestCase(nameof(Standard.VersionNumber))]
        [TestCase(nameof(Standard.Version))]
        public void Should_Add_Failure_When_Required_Field_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Standard>(propertyName);

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
            var optionsJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Options));
            var optionsUnstructuredTemplateJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.OptionsUnstructuredTemplate));

            var importedStandard = CreateValidStandard();
            importedStandard.Options = new Settable<List<Option>>();
            importedStandard.OptionsUnstructuredTemplate = new Settable<List<string>>();
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{optionsJsonPropertyName} or {optionsUnstructuredTemplateJsonPropertyName}'"));
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

        [TestCase(nameof(EqaProvider.ContactAddress))]
        [TestCase(nameof(EqaProvider.ContactEmail))]
        [TestCase(nameof(EqaProvider.ContactName))]
        [TestCase(nameof(EqaProvider.ProviderName))]
        [TestCase(nameof(EqaProvider.WebLink))]
        public void Should_Add_Failure_When_EqaProvider_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<EqaProvider>(propertyName);
            var eQAProviderJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider));

            var importedStandard = CreateValidStandard();
            typeof(EqaProvider).GetProperty(propertyName)?.SetValue(importedStandard.EqaProvider.Value, Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(EqaProvider).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{eQAProviderJsonPropertyName}.{jsonPropertyName}'"));
        }

        [TestCase(nameof(Option.OptionId))]
        [TestCase(nameof(Option.Title))]
        public void Should_Add_Failure_When_Option_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Option>(propertyName);
            var optionsJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Options));

            var importedStandard = CreateValidStandard();
            importedStandard.Options.Value.Add(new Option { OptionId = new Settable<Guid>(Guid.NewGuid()), Title = new Settable<string>("Title") });
            typeof(Option).GetProperty(propertyName)?.SetValue(importedStandard.Options.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Option).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{optionsJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Skill.SkillId))]
        [TestCase(nameof(Skill.Detail))]
        public void Should_Add_Failure_When_Skill_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Skill>(propertyName);
            var skillsJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Skills));

            var importedStandard = CreateValidStandard();
            importedStandard.Skills.Value.Add(new Skill { SkillId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Skill).GetProperty(propertyName)?.SetValue(importedStandard.Skills.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Skill).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{skillsJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Duty.DutyId))]
        [TestCase(nameof(Duty.DutyDetail))]
        [TestCase(nameof(Duty.IsThisACoreDuty))]
        [TestCase(nameof(Duty.MappedKnowledge))]
        [TestCase(nameof(Duty.MappedBehaviour))]
        [TestCase(nameof(Duty.MappedOptions))]
        [TestCase(nameof(Duty.MappedSkills))]
        public void Should_Add_Failure_When_Duty_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Duty>(propertyName);
            var dutiesJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Duties));

            var importedStandard = CreateValidStandard();
            importedStandard.Duties.Value.Add(new Duty
            {
                DutyId = new Settable<Guid>(Guid.NewGuid()),
                DutyDetail = new Settable<string>("DutyDetail"),
                IsThisACoreDuty = new Settable<long>(0),
                MappedKnowledge = new Settable<List<Guid>>(new List<Guid>()),
                MappedBehaviour = new Settable<List<Guid>>(new List<Guid>()),
                MappedOptions = new Settable<List<Guid>>(new List<Guid>()),
                MappedSkills = new Settable<List<Guid>>(new List<Guid>())
            });
            typeof(Duty).GetProperty(propertyName)?.SetValue(importedStandard.Duties.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Duty).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{dutiesJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Behaviour.BehaviourId))]
        [TestCase(nameof(Behaviour.Detail))]
        public void Should_Add_Failure_When_Behaviour_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Behaviour>(propertyName);
            var behavioursJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Behaviours));

            var importedStandard = CreateValidStandard();
            importedStandard.Behaviours.Value.Add(new Behaviour { BehaviourId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Behaviour).GetProperty(propertyName)?.SetValue(importedStandard.Behaviours.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Behaviour).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{behavioursJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(Knowledge.KnowledgeId))]
        [TestCase(nameof(Knowledge.Detail))]
        public void Should_Add_Failure_When_Knowledge_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<Knowledge>(propertyName);
            var knowledgesJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.Knowledges));

            var importedStandard = CreateValidStandard();
            importedStandard.Knowledges.Value.Add(new Knowledge { KnowledgeId = new Settable<Guid>(Guid.NewGuid()), Detail = new Settable<string>("Detail") });
            typeof(Knowledge).GetProperty(propertyName)?.SetValue(importedStandard.Knowledges.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(Knowledge).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{knowledgesJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        [TestCase(nameof(RegulationDetail.Name))]
        [TestCase(nameof(RegulationDetail.Approved))]
        public void Should_Add_Failure_When_RegulationDetail_Property_Is_Missing(string propertyName)
        {
            // Arrange
            var jsonPropertyName = GetJsonPropertyName<RegulationDetail>(propertyName);
            var regulationDetailJsonPropertyName = GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail));

            var importedStandard = CreateValidStandard();
            importedStandard.RegulationDetail.Value.Add(new RegulationDetail { Name = new Settable<string>("Name"), Approved = new Settable<bool>(true) });
            typeof(RegulationDetail).GetProperty(propertyName)?.SetValue(importedStandard.RegulationDetail.Value[0], Activator.CreateInstance(typeof(Settable<>).MakeGenericType(typeof(RegulationDetail).GetProperty(propertyName).PropertyType.GenericTypeArguments[0])));
            var importedStandards = new List<Standard> { importedStandard };

            // Act
            var result = _sut.TestValidate(importedStandards);

            // Assert
            result.Errors.Should().Contain(error => error.ErrorMessage.Contains($"E1001: {importedStandard.ReferenceNumber} version {importedStandard.Version} has missing fields '{regulationDetailJsonPropertyName}[0].{jsonPropertyName}'"));
        }

        private static string GetJsonPropertyName<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            var jsonPropertyAttribute = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                                                .Cast<JsonPropertyAttribute>()
                                                .FirstOrDefault();

            return jsonPropertyAttribute?.PropertyName ?? propertyName;
        }
    }
}
