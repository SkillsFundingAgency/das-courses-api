using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class KsbResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public static explicit operator KsbResponse(Ksb ksb)
        {
            return new KsbResponse
            {
                Id = ksb.Id,
                Type = ksb.Type.ToString(),
                Description = ksb.Detail,
            };
        }
    }
}
