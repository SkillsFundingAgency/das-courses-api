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
    public class iFateHealthCheck
    {
        private const string HealthCheckResultDescription = "iFate Input Health Check";
        private IImportAuditRepository _importData;

        public iFateHealthCheck(IImportAuditRepository importData)
        {
            _importData = importData;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();

            var healthStatusDegraded = HealthStatus.Degraded;
            var latestIfateData = _importData.GetLastImportByType(ImportType.IFATEImport);
            var iFateDataImportTimeStart = latestIfateData.Result.TimeStarted;

            // AC timeframes
            var dateNow = DateTime.UtcNow;
            var overTwentyFiveHours = dateNow.AddHours(-25);
            var twoWeeks = dateNow.AddDays(-14);

            // Is start time over 25 hours?
            var timeResultIfate = DateTime.Compare(iFateDataImportTimeStart, overTwentyFiveHours);
            // Compare start time to two weeks and an hour ago
            var timeResultIfateTwoWeeksAndAnHour = DateTime.Compare(iFateDataImportTimeStart, twoWeeks.AddHours(-1));

            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            // AC1 If course data load is over 25 hours old or rows imported is zero then the health is shown as degraded.
            // AC3 If course data is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (timeResultIfate > 0 || latestIfateData.Result.RowsImported == 0 || timeResultIfateTwoWeeksAndAnHour > 0)
            {
                return new HealthCheckResult(healthStatusDegraded, "Course data load is over 25 hours old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC4 If the courses data is under 2 weeks and an hour old then the site is shown as healthy
            if (timeResultIfateTwoWeeksAndAnHour < 0)
            {
                return HealthCheckResult.Healthy("IFATE and LARS data are both under two weeks (and an hour) old", new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } });

        }
    }
}
