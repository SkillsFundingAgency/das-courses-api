using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ShortCourseStandardsAttribute : CustomizeAttribute
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

            return new ShortCourseStandardsCustomization();
        }
    }

    public class ShortCourseStandardsCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<LarsStandard>(composer =>
                composer
                    .With(standard => standard.EffectiveFrom, DateTime.Today.AddDays(-1))
                    .Without(standard => standard.LastDateStarts));

            fixture.Customize<Standard>(composer =>
                composer
                    .With(standard => standard.Status, "Approved for delivery")
                    .With(standard => standard.ApprenticeshipType, ApprenticeshipType.Apprenticeship)
                    .With(standard => standard.CourseType, CourseType.ShortCourse));
        }
    }
}
