using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.HealthCheck
{
    public class SkillsEnglandServiceHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Skills England Input Health Check";
        private readonly IImportAuditRepository _importData;

        public SkillsEnglandServiceHealthCheck(IImportAuditRepository importData)
        {
            _importData = importData;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();
            var latestSkillsEnglandData = await _importData.GetLastImportByType(ImportType.SkillsEnglandImport);

            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            if (DateTime.UtcNow >= latestSkillsEnglandData.TimeStarted.AddHours(25))
            {
                return new HealthCheckResult(HealthStatus.Degraded, "Course data load is over 25 hours old", null, new Dictionary<string, object> { { "Duration", durationString } });
            }

            if (latestSkillsEnglandData.RowsImported == 0)
            {
                return new HealthCheckResult(HealthStatus.Degraded, "Course data load has imported zero rows", null, new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } });
        }
    }
}
