using System;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public class QueueBackgroundRequestResult
    {
        public bool Queued { get; init; }

        public Guid RequestId { get; init; }

        public string Reason { get; init; }
    }
}
