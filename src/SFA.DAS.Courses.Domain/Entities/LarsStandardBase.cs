using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandardBase
    {
        public Guid Id { get; set; }
        public int StandardId { get; set; }
        public int Version { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateStarts { get; set; }
    }
}