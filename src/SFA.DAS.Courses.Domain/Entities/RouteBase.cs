using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class RouteBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Standard> Standards { get; set; }
    }
}