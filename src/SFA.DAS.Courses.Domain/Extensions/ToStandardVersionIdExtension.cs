namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class ToStandardVersionIdExtension
    {
        public static string ToStandardVersionId(this string ifateReferenceNumber, decimal? version)
        {
            var derivedVersion = version.HasValue && version != 0 ? version.Value : 1;
            return $"{ifateReferenceNumber.Trim()}_{derivedVersion:0.0}";
        }
    }
}