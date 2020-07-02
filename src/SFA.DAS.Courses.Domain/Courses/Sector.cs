using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Sector
    {
        public Guid Id { get; set; }
        public string Route { get; set; }

        public static implicit operator Sector (Domain.Entities.Sector sector)
        {
            return new Sector
            {
                Id = sector.Id,
                Route = sector.Route
            };
        }
    }
}