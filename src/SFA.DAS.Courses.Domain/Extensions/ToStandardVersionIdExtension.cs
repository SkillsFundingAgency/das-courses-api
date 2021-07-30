namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class ToStandardVersionIdExtension
    {
        public static string ToStandardVersionId(this string ifateReferenceNumber, string version)
        {
            version = version.ToBaselineVersion();
            return $"{ifateReferenceNumber.Trim()}_{version}";
        }
    }
}
