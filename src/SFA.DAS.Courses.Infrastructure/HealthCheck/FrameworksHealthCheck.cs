using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.HealthCheck
{
    public class FrameworksHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Framework Data Health Check";
        private readonly IImportAuditRepository _repository;

        public FrameworksHealthCheck (IImportAuditRepository repository)
        {
            _repository = repository;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            
            var auditRecord = await _repository.GetLastImportByType(ImportType.FrameworkImport);
            
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            
            if (auditRecord == null)
            {
                return new HealthCheckResult(HealthStatus.Degraded, "No framework data loaded", null, new Dictionary<string, object> { { "Duration", durationString } });
            }
            
            return HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object>
            {
                { "Duration", durationString },
                { "FileName", auditRecord.FileName.Split('/').Last() }
            });
        }
    }
}