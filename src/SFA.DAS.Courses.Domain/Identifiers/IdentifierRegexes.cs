using System.Text.RegularExpressions;

namespace SFA.DAS.Courses.Domain.Identifiers
{
    public static class IdentifierRegexes
    {
        // The expected format is e.g. ZSC00001 but it can be empty and the 'SC' is not confirmed currently
        public static readonly Regex ShortCourseLarsCode =
            new("^(?:Z[A-Z0-9]{7})?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex StandardReferenceNumber =
            new(@"^ST\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex FoundationReferenceNumber =
            new(@"^FA\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex ShortCourseReferenceNumber =
            new(@"^AU\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
