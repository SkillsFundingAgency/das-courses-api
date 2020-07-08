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

            // What time is it now?
            var dateNow = DateTime.UtcNow;

            // When did IFATE data import start?
            var iFateDataImportTimeStart = latestIfateData.Result.TimeStarted;

            // Time stamp: over 25 hours ago
            var overTwentyFiveHours = dateNow.AddHours(-25);
            // Time stamp: two weeks ago
            var overTwoWeeks = dateNow.AddDays(-14);

            // Compare start time to now
            var timeResultIfate = DateTime.Compare(iFateDataImportTimeStart, overTwentyFiveHours);

            // Compare start time to two weeks ago
            var timeResultIfateTwoWeeks = DateTime.Compare(iFateDataImportTimeStart, overTwoWeeks);

            // Compare start time to two weeks ago (minus an hour)
            var timeResultIfateTwoWeeksMinusOneHour = DateTime.Compare(iFateDataImportTimeStart, overTwoWeeks);

            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            // AC1 If course data load is over 25 hours old or rows imported is zero then the health is shown as degraded.
            if (timeResultIfate > 0 || latestIfateData.Result.RowsImported == 0)
            {
                // show as degraded
                return new HealthCheckResult(healthStatusDegraded, "Course data load is over 25 hours old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC3 If course data is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (timeResultIfateTwoWeeks > 0 || latestIfateData.Result.RowsImported == 0)
            {
                // show as degraded
                return new HealthCheckResult(healthStatusDegraded, "IFATE data is over two weeks (and an hour) old or rows imported are zero", null, new Dictionary<string, object> { { "Dictionary", durationString } });
            }

            // AC4 If the courses data is under 2 weeks and an hour old then the site is shown as healthy
            if (timeResultIfateTwoWeeksMinusOneHour < 0)
            {
                // show as healthy
                return HealthCheckResult.Healthy("IFATE and LARS data are both under two weeks (and an hour) old", new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } });

        }
    }
}
