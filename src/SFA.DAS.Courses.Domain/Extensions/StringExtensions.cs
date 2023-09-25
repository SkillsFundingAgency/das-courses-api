using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating whether any substring from the given collection occurs within this string.
        /// Similar to <seealso cref="String.Contains(string)"/>, but takes a collection of strings as a parameter.
        /// </summary>
        /// <param name="source">The string to check</param>
        /// <param name="subStrings">The collection of substrings to check for occurrence</param>
        /// <param name="stringComparisonRules">The <seealso cref="StringComparison"/> method to use.</param>
        /// <returns><see langword="true"/> if any item in <paramref name="subStrings"/> occurs as a substring within this string; otherwise <see langword="false"/></returns>
        public static bool ContainsSubstringIn(this string source, IEnumerable<string> subStrings, StringComparison stringComparisonRules)
        {
            return subStrings.Any(s => source.Contains(s, stringComparisonRules));
        }
    }
}
