using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.HealthCheck
{
    public class InstituteOfApprenticeshipServiceHealthCheckHealthCheck
    {
        private const string HealthCheckResultDescription = "iFate Input Health Check";
        private IImportAuditRepository _importData;

        public InstituteOfApprenticeshipServiceHealthCheckHealthCheck(IImportAuditRepository importData)
        {
            _importData = importData;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();

            var healthStatusDegraded = HealthStatus.Degraded;
            var latestIfateData = await _importData.GetLastImportByType(ImportType.IFATEImport);

            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            if (DateTime.UtcNow >= latestIfateData.TimeStarted.AddHours(25) || latestIfateData.RowsImported == 0)
            {
                return new HealthCheckResult(healthStatusDegraded, "Course data load is over 25 hours old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } });
        }
    }
}
