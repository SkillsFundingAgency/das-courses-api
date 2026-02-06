using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ApprenticeshipStandardsRetiredAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.ParameterType.IsAssignableFrom(typeof(IEnumerable<Standard>)))
            {
                throw new ArgumentException(nameof(parameter));
            }

            return new ApprenticeshipStandardsRetiredCustomization();
        }
    }

    public class ApprenticeshipStandardsRetiredCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Standard>(composer =>
                composer
                    .With(standard => standard.Status, "Retired")
                    .With(standard => standard.ApprenticeshipType, ApprenticeshipType.Apprenticeship)
                    .With(standard => standard.CourseType, CourseType.Apprenticeship));
        }
    }
}
