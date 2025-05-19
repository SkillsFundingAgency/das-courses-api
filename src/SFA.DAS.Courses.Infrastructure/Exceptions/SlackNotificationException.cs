using System;

namespace SFA.DAS.Courses.Infrastructure.Exceptions
{
    public class SlackNotificationException : Exception
    {
        public SlackNotificationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
