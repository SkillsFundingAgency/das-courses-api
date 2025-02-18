namespace SFA.DAS.Courses.Domain.Configuration
{
    public static class Constants
    {
        public static string QualificationSectorSubjectAreaUrl => "https://qualification-sector-subject-area.register.gov.uk/";
        public static string LarsBasePageUrl => "https://submit-learner-data.service.gov.uk";
        public static string LarsDownloadPageUrl => $"{LarsBasePageUrl}/find-a-learning-aim/DownloadData";
        public static string LarsStandardsFileName => "Standard.csv";
        public static string LarsApprenticeshipFundingFileName => "ApprenticeshipFunding.csv";
        public static string LarsSectorSubjectAreaTier2FileName => "SectorSubjectAreaTier2.csv";
        public static string LarsSectorSubjectAreaTier1FileName => "SectorSubjectAreaTier1.csv";
        public static string SlackStartUploadUrl => "https://slack.com/api/files.getUploadURLExternal";
        public static string SlackCompleteUploadUrl => "https://slack.com/api/files.completeUploadExternal";
    }
}
