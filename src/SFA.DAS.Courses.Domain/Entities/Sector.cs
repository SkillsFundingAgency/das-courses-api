namespace SFA.DAS.Courses.Domain.Entities
{
    public class Sector : SectorBase
    {
        public static implicit operator Sector(SectorImport sectorImport)
        {
            return new Sector
            {
                Id = sectorImport.Id,
                Route = sectorImport.Route
            };
        }
    }
}