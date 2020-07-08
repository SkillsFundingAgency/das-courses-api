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
            var latestLarsData = _importData.GetLastImportByType(ImportType.LarsImport);
            var larsDataImportTimeStart = latestLarsData.Result.TimeStarted;

            // AC timeframes
            var dateNow = DateTime.UtcNow;
            var twentyFiveHours = dateNow.AddHours(-25);
            var twoWeeksAndOneHour = dateNow.AddDays(-14).AddHours(1);

            // Compare start time to now
            var timeResultLars = DateTime.Compare(larsDataImportTimeStart, twentyFiveHours);
            // Compare start time to two weeks ago
            var timeResultLarsTwoWeeks = DateTime.Compare(larsDataImportTimeStart, twoWeeksAndOneHour);
            
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            /***** START OF FUNCTIONALITY ******/

            // AC2 If LARS data load is over 25 hours old then the health is shown as degraded
            // AC3 If LARS data is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (timeResultLarsTwoWeeks > 0 || latestLarsData.Result.RowsImported == 0 || timeResultLars < 0)
            {
                // show as degraded
                return new HealthCheckResult(healthStatusDegraded, "LARS and IFATE data is over two weeks (and an hour) old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC4 If the  LARS data is under 2 weeks and an hour old then the site is shown as healthy
            if (timeResultLarsTwoWeeks < 0)
            {
                // show as healthy
                return HealthCheckResult.Healthy("IFATE and LARS data are both under two weeks (and an hour) old", new Dictionary<string, object> { { "Duration", durationString } });
            }

            // AC5 If the SQL database is unable to connect then it is shown as unhealthy
            // AC6 If the SQL database is able to connect then it is shown as healthy

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { {"Duration", durationString } });
        }
    }
}
