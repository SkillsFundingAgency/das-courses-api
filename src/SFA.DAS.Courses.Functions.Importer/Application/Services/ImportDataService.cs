using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SFA.DAS.Courses.Functions.Importer.Domain.Configuration;
using SFA.DAS.Courses.Functions.Importer.Domain.Interfaces;

namespace SFA.DAS.Courses.Functions.Importer.Application.Services
{
    public class ImportDataService : IImportDataService
    {
        private readonly HttpClient _client;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly ImporterEnvironment _importerEnvironment;
        private readonly ImporterConfiguration _configuration;

        public ImportDataService(  HttpClient client, IOptions<ImporterConfiguration> configuration,
            IAzureClientCredentialHelper azureClientCredentialHelper, ImporterEnvironment importerEnvironment)
        {
            _client = client;
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _importerEnvironment = importerEnvironment;
            _configuration = configuration.Value;
        }

        public void Import()
        {
            if (!_importerEnvironment.EnvironmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                var token = _azureClientCredentialHelper.GetAccessTokenAsync().Result;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);    
            }
            
            _client.PostAsync($"{_configuration.Url}ops/dataload", null).ConfigureAwait(false);
        }
        
    }
}