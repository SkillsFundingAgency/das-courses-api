using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ShortCourseStandardsInvalidAttribute : CustomizeAttribute
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

            return new ShortCourseStandardsInvalidCustomization();
        }
    }

    public class ShortCourseStandardsInvalidCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new StandardCustomization(
                status: "Approved for delivery",
                apprenticeshipType: ApprenticeshipType.ApprenticeshipUnit,
                courseType: CourseType.ShortCourse,
                version: "1.0",
                approvedForDelivery: DateTime.UtcNow.AddDays(-10),
                effectiveFrom: DateTime.UtcNow.AddDays(1), // with ShortCourseDates which are not effective yet
                effectiveTo: null,
                lastDateStarts: null));
        }
    }
}
