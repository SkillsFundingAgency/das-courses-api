using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
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

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var timer = Stopwatch.StartNew();
            var latestLarsData = _importData.GetLastImportByType(ImportType.LarsImport);
            var latestIfateData = _importData.GetLastImportByType(ImportType.IFATEImport);

            var dateNow = DateTime.UtcNow;
            // When did LARS data import start?
            var larsDataImportTimeStart = latestLarsData.Result.TimeStarted;
            // When did LARS data import start?
            var iFateDataImportTimeStart = latestIfateData.Result.TimeStarted;
            var overTwentyFiveHours = dateNow.AddHours(-25);
            var overTwoWeeks = dateNow.AddDays(-14);

            // Compare start time to now
            var timeResultLars = DateTime.Compare(larsDataImportTimeStart, overTwentyFiveHours);
            var timeResultIfate = DateTime.Compare(iFateDataImportTimeStart, overTwentyFiveHours);

            var timeResultIfateTwoWeeks = DateTime.Compare(iFateDataImportTimeStart, overTwoWeeks);
            var timeResultLarsTwoWeeks = DateTime.Compare(iFateDataImportTimeStart, overTwoWeeks);

            // AC1 If course data load is over 25 hours old or rows imported is zero then the health is shown as degraded.
            if (timeResultIfate < 0 || latestIfateData.Result.RowsImported == 0)
            {
                // show as degraded
            }

            // AC3 If course and LARS data is over 2 weeks and an hour old or rows imported is zero then the health is shown as degraded
            if (timeResultIfateTwoWeeks < 0 || timeResultLarsTwoWeeks < 0 || latestIfateData.Result.RowsImported == 0 || latestLarsData.Result.RowsImported == 0)
            {
                // show as degraded
            }

        }
    }
}
