using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class Sector
    {
        public Guid Id { get; set; }
        public string Route { get; set; }
        
        public virtual ICollection<Standard> Standards { get; set; }
    }
}