using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetSectorResponse
    {
        public Guid Id { get ; set ; }

        public string Route { get ; set ; }

        public static implicit operator GetSectorResponse(Sector sector)
        {
            return new GetSectorResponse
            {
                Id = sector.Id,
                Route = sector.Route
            };
        }
    }
}