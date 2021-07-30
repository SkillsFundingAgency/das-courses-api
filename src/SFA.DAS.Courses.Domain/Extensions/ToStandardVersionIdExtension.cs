namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class ToStandardVersionIdExtension
    {
        public static string ToStandardVersionId(this string ifateReferenceNumber, string version)
        {
            if(string.IsNullOrWhiteSpace(version) || !decimal.TryParse(version, out var decimalVersion) || decimalVersion < 1)
            {
                return $"{ifateReferenceNumber.Trim()}_1.0";
            }

            return $"{ifateReferenceNumber.Trim()}_{version}";
        }
    }
}
