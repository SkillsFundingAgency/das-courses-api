using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.Courses.Domain.TestHelper.AutoFixture
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StandardInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public StandardInlineAutoDataAttribute(params object[] arguments) : base(() =>
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            fixture.Customizations.Add(new SettableSpecimenBuilder());

            return fixture;
        }, arguments)
        {
        }
    }

}
