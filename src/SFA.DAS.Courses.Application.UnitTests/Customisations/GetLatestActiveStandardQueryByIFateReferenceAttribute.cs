using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;

namespace SFA.DAS.Courses.Application.UnitTests.Customisations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GetLatestActiveStandardQueryByIFateReferenceAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.ParameterType.IsAssignableFrom(typeof(IEnumerable<GetLatestActiveStandardQuery>)))
            {
                throw new ArgumentException(nameof(parameter));
            }

            return new GetLatestActiveStandardQueryByIFateReferenceCustomisation();
        }
    }

    public class GetLatestActiveStandardQueryByIFateReferenceCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Inject(new GetLatestActiveStandardQuery(fixture.Create<string>().Substring(0,6)));
        }
    }
}
