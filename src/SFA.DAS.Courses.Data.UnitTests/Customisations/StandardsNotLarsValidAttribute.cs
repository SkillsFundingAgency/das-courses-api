using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StandardsNotLarsValidAttribute : CustomizeAttribute
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

            return new StandardsNotLarsValidCustomisation();
        }
    }

    public class StandardsNotLarsValidCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // the Standard is valid
            fixture.Customize<Standard>(composer =>
                composer.With(standard => standard.Status, "Approved for delivery")
                        .With(standard => standard.ApprenticeshipType, ApprenticeshipType.Apprenticeship.ToString()));

            // but its related LarsStandard is not effective yet
            fixture.Customize<LarsStandard>(composer =>
                composer.With(standard => standard.EffectiveFrom, DateTime.Today.AddDays(1)));
        }
    }
}
