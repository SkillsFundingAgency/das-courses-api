namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class ToStandarUIdExtension
    {
        public static string ToStandardUId(this string ifateReferenceNumber, string version)
        {
            version = version.ToBaselineVersion();
            return $"{ifateReferenceNumber.Trim()}_{version}";
        }
    }
}
