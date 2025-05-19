using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.Courses.Domain.TestHelper.AutoFixture
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StandardAutoDataAttribute : AutoDataAttribute
    {
        public StandardAutoDataAttribute() : base(() =>
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            fixture.Customizations.Add(new SettableSpecimenBuilder());

            return fixture;
        })
        {
        }
    }

}
