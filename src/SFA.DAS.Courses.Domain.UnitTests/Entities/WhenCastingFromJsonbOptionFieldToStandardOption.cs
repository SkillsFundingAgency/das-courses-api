using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromJsonbOptionFieldToStandardOption
    {
        [Test]
        public void Then_The_Fields_Are_Mapped_Correctly()
        {
            var options = new[]
            {
                StandardOption.Create(
                    System.Guid.NewGuid(),
                    "TheOption", new List<Ksb>
                    {
                        Ksb.Knowledge(1, "k1"),
                        Ksb.Knowledge(2, "k2"),
                        Ksb.Knowledge(3, "k3"),
                    })
            };

            var json = ValueConversionExtensions.Serialise(options);
            var converted = ValueConversionExtensions.Deserialise<List<StandardOption>>(json);
            converted.Should().BeEquivalentTo(options, c => c.IncludingAllDeclaredProperties());
        }
    }
}
