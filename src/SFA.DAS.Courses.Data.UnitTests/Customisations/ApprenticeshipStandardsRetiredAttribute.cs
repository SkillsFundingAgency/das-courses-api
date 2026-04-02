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

            if (!typeof(IEnumerable<Standard>).IsAssignableFrom(parameter.ParameterType))
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
            fixture.Customize(new StandardCustomization(
                status: "Retired",
                apprenticeshipType: ApprenticeshipType.Apprenticeship,
                courseType: CourseType.Apprenticeship,
                version: "1.0",
                approvedForDelivery: DateTime.UtcNow.AddDays(-10),
                effectiveFrom: DateTime.UtcNow.Date.AddDays(10), // with a LarsStandard which is no longer effective
                effectiveTo: DateTime.UtcNow.Date.AddDays(-1),
                lastDateStarts: DateTime.UtcNow.AddDays(-2)));    
        }
    }
}
