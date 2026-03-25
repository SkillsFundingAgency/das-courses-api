using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.TestHelper.AutoFixture
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StandardInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public StandardInlineAutoDataAttribute(params object[] arguments)
            : base(() => CreateFixture(), arguments)
        {
        }

        public StandardInlineAutoDataAttribute(
            ApprenticeshipType apprenticeshipType,
            params object[] arguments)
            : base(() => CreateFixture(apprenticeshipType), arguments)
        {
        }

        public StandardInlineAutoDataAttribute(
            ApprenticeshipType apprenticeshipType,
            string version,
            params object[] arguments)
            : base(() => CreateFixture(apprenticeshipType, version), arguments)
        {
        }

        private static IFixture CreateFixture()
        {
            var fixture = FixtureBuilder.RecursiveMoqFixtureFactory();
            fixture.Customizations.Add(new SettableSpecimenBuilder());

            return fixture;
        }

        private static IFixture CreateFixture(
            ApprenticeshipType apprenticeshipType,
            string? version = null)
        {
            var fixture = CreateFixture();
            var counter = 1;

            fixture.Customize<Standard>(c =>
            {
                var composer = c
                    .With(x => x.ApprenticeshipType, apprenticeshipType)
                    .With(x => x.Version, version);
                    

                if (apprenticeshipType == ApprenticeshipType.Apprenticeship)
                {
                    composer = composer
                        .With(x => x.CourseType, CourseType.Apprenticeship)
                        .With(x => x.StandardUId, $"ST{counter++:0000}_{version}")
                        .With(x => x.LarsCode, counter.ToString());
                }
                else if (apprenticeshipType == ApprenticeshipType.FoundationApprenticeship)
                {
                    composer = composer
                        .With(x => x.CourseType, CourseType.Apprenticeship)
                        .With(x => x.StandardUId, $"FA{counter++:0000}_{version}")
                        .With(x => x.LarsCode, counter.ToString());
                }
                else if (apprenticeshipType == ApprenticeshipType.ApprenticeshipUnit)
                {
                    composer = composer
                        .With(x => x.CourseType, CourseType.ShortCourse)
                        .With(x => x.StandardUId, $"AU{counter++:0000}_{version}")
                        .With(x => x.LarsCode, $"ZSC{counter++:00000}");
                }

                return composer;
            });


            return fixture;
        }
    }
}
