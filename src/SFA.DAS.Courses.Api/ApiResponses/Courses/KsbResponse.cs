using System;
using OpenTelemetry.Resources;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class KsbResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public static explicit operator KsbResponse(Ksb source)
        {
            if (source == null)
                return null;

            return new KsbResponse
            {
                Id = source.Id,
                Type = source.Type.ToString(),
                Description = source.Detail,
            };
        }
    }
}
