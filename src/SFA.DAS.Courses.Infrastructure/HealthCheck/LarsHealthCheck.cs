using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.HealthCheck
{
    public class LarsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "LARS Input Health Check";
        private readonly IImportAuditRepository _importData;

        public LarsHealthCheck(IImportAuditRepository importData)
        {
            _importData = importData;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();
            var latestLarsData = await _importData.GetLastImportByType(ImportType.LarsImport);
            
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            if (DateTime.UtcNow >= latestLarsData.TimeStarted.AddDays(14).AddHours(1))
            {
                return new HealthCheckResult(HealthStatus.Degraded, "LARS data is over two weeks (and an hour) old", null, new Dictionary<string, object> { { "Duration", durationString } });
            }

            if (latestLarsData.RowsImported == 0)
            {
                return new HealthCheckResult(HealthStatus.Degraded, "LARS data has imported zero rows", null, new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object>
            {
                {"Duration", durationString },
                {"FileName", latestLarsData.FileName.Split('/').Last()}
            });
        }
    }
}
