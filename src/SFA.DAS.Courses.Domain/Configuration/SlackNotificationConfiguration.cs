namespace SFA.DAS.Courses.Domain.Configuration
{
    public class SlackNotificationConfiguration
    {
        public string BotUserOAuthToken { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
    }
}
