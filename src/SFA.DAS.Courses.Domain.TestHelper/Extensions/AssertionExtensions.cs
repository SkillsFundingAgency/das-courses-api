using FluentAssertions;
using FluentAssertions.Equivalency;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;


namespace SFA.DAS.Courses.Domain.TestHelper.Extensions
{
    public static class AssertionExtensions
    {
        public static void ShouldBeEquivalentToWithSettableHandling<T>(
            this IEnumerable<T> actual,
            IEnumerable<T> expected,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> optionsConfigurator)
        {
            actual.Should().BeEquivalentTo(expected, options =>
            {
                options = optionsConfigurator(options);

                return options
                    .Using<object>(ctx =>
                    {
                        if (ctx.Subject != null && ctx.Expectation != null)
                        {
                            Type subjectType = ctx.Subject.GetType();
                            Type expectationType = ctx.Expectation.GetType();

                            if (subjectType.IsGenericType && subjectType.GetGenericTypeDefinition() == typeof(Settable<>) &&
                                expectationType.IsGenericType && expectationType.GetGenericTypeDefinition() == typeof(Settable<>))
                            {
                                object subjectValue = subjectType.GetProperty("Value")?.GetValue(ctx.Subject);
                                object expectationValue = expectationType.GetProperty("Value")?.GetValue(ctx.Expectation);

                                subjectValue.Should().BeEquivalentTo(expectationValue,
                                    options => options.RespectingRuntimeTypes(),
                                    $"Mismatch in {ctx.SelectedNode.Name}");
                            }
                            else
                            {
                                ctx.Subject.Should().BeEquivalentTo(ctx.Expectation,
                                    options => options.RespectingRuntimeTypes(),
                                    $"Mismatch in {ctx.SelectedNode.Name}");
                            }
                        }
                    })
                    .When(info => info.RuntimeType.IsGenericType &&
                                  info.RuntimeType.GetGenericTypeDefinition() == typeof(Settable<>));
            });
        }
    }
}
