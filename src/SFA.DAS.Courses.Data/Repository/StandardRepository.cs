using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardRepository : IStandardRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public StandardRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<int> Count(StandardFilter filter)
        {
            // Tweak to the count query to perform a count rather than query actual fields.
            // The in memory filter causes this query to become more resource intensive
            // To get around that for the Active and Active Available filters
            // We perform the normal filter that we can, then select distinct lars code to get latest version count
            int count;
            var standards = _coursesDataContext.Standards.FilterStandards(filter);
            switch (filter)
            {
                case StandardFilter.Active:
                case StandardFilter.ActiveAvailable:
                    count = await standards.Select(c => c.LarsCode).Distinct().CountAsync();
                    break;
                default:
                    count = await standards.Select(c => c.StandardUId).CountAsync();
                    break;
            }

            return count;
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.Standards.RemoveRange(_coursesDataContext.Standards);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<int> InsertMany(IEnumerable<Standard> standards)
        {
            await _coursesDataContext.Standards.AddRangeAsync(standards);
            return await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<List<Standard>> GetActiveStandardsByIfateReferenceNumbers(List<string> ifateReferenceNumbers)
        {
            var query = GetBaseStandardQuery()
                .FilterStandards(StandardFilter.ActiveAvailable)
                .Where(s => ifateReferenceNumbers.Contains(s.IfateReferenceNumber));
            var standards = await query.ToListAsync();

            var filteredStandards = standards.InMemoryFilterIsLatestVersion(StandardFilter.ActiveAvailable);

            return (await IncludeApprenticeshipFunding(filteredStandards)).ToList();
        }

        public async Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber)
        {
            var standards = await GetFullBaseStandardQuery()
                .FilterStandards(StandardFilter.Active)
                .Where(c => c.IfateReferenceNumber.Equals(iFateReferenceNumber)).ToListAsync();

            // In Memory Filter for get latest version due to limitations in EF query translation
            // into expression tree
            var standard = standards.InMemoryFilterIsLatestVersion(StandardFilter.Active).SingleOrDefault();

            if (standard is null) return null;

            return (await IncludeApprenticeshipFunding(new List<Standard> { standard })).First();
        }

        public async Task<Standard> GetLatestActiveStandard(string larsCode)
        {
            var standards = await GetFullBaseStandardQuery()
                .FilterStandards(StandardFilter.Active)
                .Where(c => c.LarsCode == larsCode)
                .ToListAsync();

            // In Memory Filter for get latest version due to limitations in EF query translation
            // into expression tree
            var standard = standards.InMemoryFilterIsLatestVersion(StandardFilter.Active).SingleOrDefault();

            if (standard is null) return null;

            return (await IncludeApprenticeshipFunding(new List<Standard> { standard })).First();
        }

        public async Task<Standard> Get(string standardUId)
        {
            var standard = await GetFullBaseStandardQuery()
                .SingleOrDefaultAsync(c => c.StandardUId.Equals(standardUId));

            if (standard is null) return null;

            return (await IncludeApprenticeshipFunding(new List<Standard> { standard })).First();
        }

        public async Task<IEnumerable<Standard>> GetStandards()
        {
            return await GetStandards(new List<int>(), new List<int>(), StandardFilter.None, true);
        }

        public async Task<IEnumerable<Standard>> GetStandards(IList<int> routeIds, IList<int> levels, StandardFilter filter, bool includeAllProperties, ApprenticeshipType? apprenticeshipType = null)
        {
            IQueryable<Standard> standards = (includeAllProperties
                    ? GetFullBaseStandardQuery()
                    : GetBaseStandardQuery())
                .FilterStandards(filter);

            if (routeIds.Count > 0)
            {
                standards = standards.Where(standard => routeIds.Contains(standard.RouteCode));
            }
            if (levels.Count > 0)
            {
                standards = standards.Where(standard => levels.Contains(standard.Level));
            }
            if (apprenticeshipType != null)
            {
                standards = standards.Where(standard => standard.ApprenticeshipType.Equals(apprenticeshipType));
            }

            var standardResults = await standards.ToListAsync();

            // Secondary filter performed in memory due to limitations in 
            // EF Core query translation on Group By selecting top row of each.
            var filtered = standardResults.InMemoryFilterIsLatestVersion(filter);

            return (await IncludeApprenticeshipFunding(filtered)).ToList();
        }

        public async Task<IEnumerable<Standard>> GetStandards(string iFateReferenceNumber)
        {
            var standards = await GetBaseStandardQuery()
                .Where(c => c.IfateReferenceNumber.Equals(iFateReferenceNumber))
                .ToListAsync();

            return (await IncludeApprenticeshipFunding(standards));
        }

        private IQueryable<Standard> GetFullBaseStandardQuery()
        {
            var query = _coursesDataContext
                .Standards
                .Include(c => c.Route)
                .Include(c => c.LarsStandard)
                    .ThenInclude(l => l.SectorSubjectArea2)
                .Include(c => c.LarsStandard)
                    .ThenInclude(l => l.SectorSubjectArea1);
            return query;
        }

        private IQueryable<Standard> GetBaseStandardQuery()
        {
            var query = _coursesDataContext
                .Standards
                .Include(c => c.Route)
                .Include(c => c.LarsStandard)
                    .ThenInclude(c => c.SectorSubjectArea2)
                .Include(c => c.LarsStandard)
                    .ThenInclude(c => c.SectorSubjectArea1)
                .Select(c => new Standard
                {
                    Status = c.Status,
                    StandardUId = c.StandardUId,
                    LarsStandard = c.LarsStandard,
                    Keywords = c.Keywords,
                    Level = c.Level,
                    CoronationEmblem = c.CoronationEmblem,
                    Route = c.Route,
                    Title = c.Title,
                    Version = c.Version,
                    IntegratedApprenticeship = c.IntegratedApprenticeship,
                    IntegratedDegree = c.IntegratedDegree,
                    LarsCode = c.LarsCode,
                    RouteCode = c.RouteCode,
                    VersionMajor = c.VersionMajor,
                    VersionMinor = c.VersionMinor,
                    ApprovedForDelivery = c.ApprovedForDelivery,
                    IfateReferenceNumber = c.IfateReferenceNumber,
                    StandardPageUrl = c.StandardPageUrl,
                    TypicalJobTitles = c.TypicalJobTitles,
                    VersionEarliestStartDate = c.VersionEarliestStartDate,
                    VersionLatestEndDate = c.VersionLatestEndDate,
                    VersionLatestStartDate = c.VersionLatestStartDate,
                    OverviewOfRole = c.OverviewOfRole,
                    RegulatedBody = c.RegulatedBody,
                    EpaoMustBeApprovedByRegulatorBody = c.EpaoMustBeApprovedByRegulatorBody,
                    ApprenticeshipType = c.ApprenticeshipType,
                    Options = c.Options,
                    IsRegulatedForProvider = c.IsRegulatedForProvider,
                    IsRegulatedForEPAO = c.IsRegulatedForEPAO
                });

            return query;
        }

        private async Task<IEnumerable<Standard>> IncludeApprenticeshipFunding(IEnumerable<Standard> standards)
        {
            if (standards is null || !standards.Any())
            {
                return standards;
            }

            var larsCodes = standards
                .Select(s => s.LarsCode)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList();

            if (larsCodes.Count == 0)
            {
                foreach (var s in standards)
                {
                    s.ApprenticeshipFunding = new List<ApprenticeshipFunding>();
                }

                return standards;
            }

            var funding = await _coursesDataContext.ApprenticeshipFunding
                .Where(f => larsCodes.Contains(f.LarsCode))
                .ToListAsync();

            var fundingByLarsCode = funding
                .GroupBy(f => f.LarsCode)
                .ToDictionary(
                    g => g.Key,
                    g => (ICollection<ApprenticeshipFunding>)g.ToList());

            foreach (var standard in standards)
            {
                if (!string.IsNullOrWhiteSpace(standard.LarsCode) &&
                    fundingByLarsCode.TryGetValue(standard.LarsCode, out var list))
                {
                    standard.ApprenticeshipFunding = list;
                }
                else
                {
                    standard.ApprenticeshipFunding = new List<ApprenticeshipFunding>();
                }
            }

            return standards;
        }
    }
}
