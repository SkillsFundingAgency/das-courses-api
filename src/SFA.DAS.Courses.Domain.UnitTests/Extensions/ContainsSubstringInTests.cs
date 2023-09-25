using System;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.StringExtensions
{
    public class ContainsSubstringInTests
    {
        [Test]
        public void NoMatches_ReturnsFalse()
        {
            var _subStrings = new string[] { "good worksman", "their tools" };
            const string s = "A bad worksman always blames his tools";
            Assert.That(s.ContainsSubstringIn(_subStrings, StringComparison.OrdinalIgnoreCase), Is.False);
        }

        [Test]
        public void OneMatch_ReturnsTrue()
        {
            var _subStrings = new string[] { "good worksman", "their tools" };
            const string s = "A bad worksman always blames their tools";
            Assert.That(s.ContainsSubstringIn(_subStrings, StringComparison.OrdinalIgnoreCase), Is.True);
        }

        [Test]
        public void MultipleMatches_ReturnsTrue()
        {
            var _subStrings = new string[] { "good worksman", "their tools" };
            const string s = "A good worksman never blames their tools";
            Assert.That(s.ContainsSubstringIn(_subStrings, StringComparison.OrdinalIgnoreCase), Is.True);
        }
    }
}
