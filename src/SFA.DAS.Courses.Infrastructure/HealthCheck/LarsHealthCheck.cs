using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Data.Repository;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.HealthCheck
{
    public class LarsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "LARS Input Health Check";
        private IImportAuditRepository _importData;



        public LarsHealthCheck(IImportAuditRepository importData)
        {
            _importData = importData;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();

            var healthStatusDegraded = HealthStatus.Degraded;
            var latestLarsData = await _importData.GetLastImportByType(ImportType.LarsImport);
            
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            // AC2 If LARS data load is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (DateTime.UtcNow >= latestLarsData.TimeStarted.AddDays(14).AddHours(1) || latestLarsData.RowsImported == 0)
            {
                return new HealthCheckResult(healthStatusDegraded, "LARS and IFATE data is over two weeks (and an hour) old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { {"Duration", durationString } });
        }
    }
}
