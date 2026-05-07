using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit4;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ShortCourseStandardsRetiredAttribute : CustomizeAttribute
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

            return new ShortCourseStandardsRetiredCustomization();
        }
    }

    public class ShortCourseStandardsRetiredCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new StandardCustomization(
                status: "Retired",
                apprenticeshipType: ApprenticeshipType.ApprenticeshipUnit,
                courseType: CourseType.ShortCourse,
                version: "1.0",
                approvedForDelivery: DateTime.UtcNow.AddDays(-10),
                effectiveFrom: DateTime.UtcNow.AddDays(-10),
                effectiveTo: DateTime.UtcNow.AddDays(-2),
                lastDateStarts: DateTime.UtcNow.AddDays(-1)));
        }
    }
}
