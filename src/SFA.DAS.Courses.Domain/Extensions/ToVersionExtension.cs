namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class ToVersionExtension
    {
        public static string ToBaselineVersion(this string version)
        {
            if(string.IsNullOrWhiteSpace(version) || !decimal.TryParse(version, out var decimalVersion) || decimalVersion < 1)
            {
                return "1.0";
            }

            return version;
        }
    }
}
