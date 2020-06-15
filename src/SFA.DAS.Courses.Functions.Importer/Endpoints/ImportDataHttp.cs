using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Courses.Functions.Importer.Domain.Configuration;
using SFA.DAS.Courses.Functions.Importer.Domain.Interfaces;

namespace SFA.DAS.Courses.Functions.Importer.Endpoints
{
    public class ImportDataHttp
    {
        private readonly IImportDataService _service;

        public ImportDataHttp (IImportDataService service)
        {
            _service = service;
        }
        
        [FunctionName("ImportDataHttp")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            await Task.Run(() => _service.Import());

            return new NoContentResult();
            
        }
    }
}