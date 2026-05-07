using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit4;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ApprenticeshipStandardsWithdrawnAttribute : CustomizeAttribute
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

            return new ApprenticeshipStandardsWithdrawnCustomization();
        }
    }

    public class ApprenticeshipStandardsWithdrawnCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new StandardCustomization(
                status: "Withdrawn",
                apprenticeshipType: ApprenticeshipType.Apprenticeship,
                courseType: CourseType.Apprenticeship,
                version: "1.0",
                approvedForDelivery: DateTime.UtcNow.AddDays(-10),
                effectiveFrom: DateTime.UtcNow.Date.AddDays(-1), // with a LarsStandard which is effective
                effectiveTo: null,
                lastDateStarts: null));
        }
    }
}
