using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit4;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ApprenticeshipStandardsNotYetApprovedAttribute : CustomizeAttribute
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

            return new ApprenticeshipStandardsNotYetApprovedCustomization();
        }
    }

    public class ApprenticeshipStandardsNotYetApprovedCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new StandardCustomization(
                status: "In development",
                apprenticeshipType: ApprenticeshipType.Apprenticeship,
                courseType: CourseType.Apprenticeship,
                version: "0.1",
                larsCode: "0",
                approvedForDelivery: null));
        }
    }
}
