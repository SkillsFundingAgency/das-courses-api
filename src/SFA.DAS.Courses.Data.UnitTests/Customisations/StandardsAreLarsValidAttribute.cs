﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StandardsAreLarsValidAttribute : CustomizeAttribute
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

            return new StandardsAreLarsValidCustomisation();
        }
    }

    public class StandardsAreLarsValidCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<LarsStandard>(composer => 
                composer.With(standard => standard.EffectiveFrom, DateTime.Today.AddDays(-1))
                        .Without(standard => standard.LastDateStarts));

            fixture.Customize<Standard>(composer =>
                composer.With(standard => standard.Status,"Approved for delivery"));
        }
    }
}
