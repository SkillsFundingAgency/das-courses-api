using System.Text.RegularExpressions;

namespace SFA.DAS.Courses.Domain.Identifiers
{
    public static class IdentifierRegexes
    {
        public static readonly Regex ShortCourseLarsCode =
            new(@"^ZSC\d{5}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex StandardReferenceNumber =
            new(@"^ST\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex FoundationReferenceNumber =
            new(@"^FA\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex ShortCourseReferenceNumber =
            new(@"^SC\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
