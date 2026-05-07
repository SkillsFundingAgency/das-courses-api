using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit4;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StandardRepositoryTestDataAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (!typeof(StandardRepositoryTestData).IsAssignableFrom(parameter.ParameterType))
            {
                throw new ArgumentException(nameof(parameter));
            }

            return new StandardRepositoryTestDataCustomization();
        }
    }

    public class StandardRepositoryTestDataCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var ifateReferenceNumberCounter = 1;

            fixture.Register(() => new StandardRepositoryTestData
            {
                ActiveValidApprenticeshipStandards = CreateStandards<ApprenticeshipStandardsLarsValidCustomization>(ref ifateReferenceNumberCounter),
                ActiveInvalidApprenticeshipStandards = CreateStandards<ApprenticeshipStandardsNotLarsValidCustomization>(ref ifateReferenceNumberCounter),
                NotYetApprovedApprenticeshipStandards = CreateStandards<ApprenticeshipStandardsNotYetApprovedCustomization>(ref ifateReferenceNumberCounter),
                WithdrawnApprenticeshipStandards = CreateStandards<ApprenticeshipStandardsWithdrawnCustomization>(ref ifateReferenceNumberCounter),
                RetiredApprenticeshipStandards = CreateStandards<ApprenticeshipStandardsRetiredCustomization>(ref ifateReferenceNumberCounter),
                ActiveValidFoundationApprenticeshipStandards = CreateStandards<FoundationApprenticeshipStandardsLarsValidCusomization>(ref ifateReferenceNumberCounter),
                ActiveValidShortCourseStandards = CreateStandards<ShortCourseStandardsValidCustomization>(ref ifateReferenceNumberCounter),
                ActiveInvalidShortCourseStandards = CreateStandards<ShortCourseStandardsInvalidCustomization>(ref ifateReferenceNumberCounter),
                NotYetApprovedShortCourseStandards = CreateStandards<ShortCourseStandardsNotYetApprovedCustomization>(ref ifateReferenceNumberCounter),
                WithdrawnShortCourseStandards = CreateStandards<ShortCourseStandardsWithdrawnCustomization>(ref ifateReferenceNumberCounter),
                RetiredShortCourseStandards = CreateStandards<ShortCourseStandardsRetiredCustomization>(ref ifateReferenceNumberCounter),
            });
        }

        private static List<Standard> CreateStandards<TCustomization>(
            ref int counter,
            int count = 3)
            where TCustomization : ICustomization, new()
        {
            var fixture = FixtureBuilder.RecursiveMoqFixtureFactory();
            fixture.Customize(new TCustomization());

            var standards = fixture.CreateMany<Standard>(count).ToList();

            int level = 1;
            foreach (var standard in standards)
            {
                standard.Level = level++;
                
                if(standard.ApprenticeshipType == ApprenticeshipType.Apprenticeship)
                {
                    standard.IfateReferenceNumber = $"ST{counter:0000}";
                }
                else if(standard.ApprenticeshipType == ApprenticeshipType.FoundationApprenticeship)
                {
                    standard.IfateReferenceNumber = $"FA{counter:0000}";
                }
                else if(standard.ApprenticeshipType == ApprenticeshipType.ApprenticeshipUnit)
                {
                    standard.IfateReferenceNumber = $"AU{counter:0000}";
                }

                if(standard.CourseType == CourseType.Apprenticeship)
                {
                    standard.LarsCode = standard.LarsCode != "0" ? counter.ToString() : standard.LarsCode;

                    if (standard.LarsCode == "0")
                    {
                        standard.LarsStandard = null;
                    }
                    
                    if(standard.LarsStandard != null)
                    {
                        standard.LarsStandard.LarsCode = standard.LarsCode;
                        standard.LarsStandard.SectorSubjectAreaTier1 = int.Parse(standard.LarsCode);
                        standard.LarsStandard.SectorSubjectArea1.SectorSubjectAreaTier1 = standard.LarsStandard.SectorSubjectAreaTier1.Value;
                        standard.LarsStandard.SectorSubjectAreaTier2 = int.Parse(standard.LarsCode);
                        standard.LarsStandard.SectorSubjectArea2.SectorSubjectAreaTier2 = standard.LarsStandard.SectorSubjectAreaTier2;
                    }
                }
                else if (standard.CourseType == CourseType.ShortCourse)
                {
                    standard.LarsCode = standard.LarsCode != string.Empty ? $"ZSC{counter:00000}" : standard.LarsCode;

                    if (standard.LarsCode == string.Empty)
                    {
                        standard.ShortCourseDates = null;
                    }

                    if (standard.ShortCourseDates != null)
                    {
                        standard.ShortCourseDates.LarsCode = standard.LarsCode;
                    }
                }
                
                counter++;
            }

            return standards;
        }
    }
}
