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
            var latestIfateData = _importData.GetLastImportByType(ImportType.IFATEImport);

            var dateNow = DateTime.UtcNow;
            // When did LARS data import start?
            var larsDataImportTimeStart = latestLarsData.Result.TimeStarted;
            // When did LARS data import start?
            var iFateDataImportTimeStart = latestIfateData.Result.TimeStarted;

            // AC timeframes
            var overTwentyFiveHours = dateNow.AddHours(-25);
            var overTwoWeeks = dateNow.AddDays(-14);
            var underTwoWeeksAndOneHour = dateNow.AddDays(-14).AddHours(1);

            // Compare start time to now
            var timeResultLars = DateTime.Compare(larsDataImportTimeStart, overTwentyFiveHours);

            // Compare start time to two weeks ago
            var timeResultLarsTwoWeeks = DateTime.Compare(larsDataImportTimeStart, underTwoWeeksAndOneHour);
            var timeResultIfateTwoWeeks = DateTime.Compare(iFateDataImportTimeStart, underTwoWeeksAndOneHour);

            // Compare start time to two weeks ago minus one hour
            var timeResultLarsTwoWeeksMinusOneHour = DateTime.Compare(larsDataImportTimeStart, overTwoWeeks);
            var timeResultIfateTwoWeeksMinusOneHour = DateTime.Compare(iFateDataImportTimeStart, overTwoWeeks);
            
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            // AC2 If LARS data load is over 25 hours old then the health is shown as degraded
            if (timeResultLars < 0)
            {
                // show degraded
                return new HealthCheckResult(healthStatusDegraded, "LARS data load is over 25 hours old", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC3 If course and LARS data is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (timeResultIfateTwoWeeks > 0 || timeResultLarsTwoWeeks > 0 || latestIfateData.Result.RowsImported == 0 || latestLarsData.Result.RowsImported == 0)
            {
                // show as degraded
                return new HealthCheckResult(healthStatusDegraded, "LARS and IFATE data is over two weeks (and an hour) old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC4 If the courses and LARS data is under 2 weeks and an hour old then the site is shown as healthy
            if (timeResultLarsTwoWeeksMinusOneHour < 0 && timeResultIfateTwoWeeksMinusOneHour < 0)
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
