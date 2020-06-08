using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.Courses.Functions.Importer.Domain.Configuration;
using SFA.DAS.Courses.Functions.Importer.Domain.Interfaces;

namespace SFA.DAS.Courses.Functions.Importer.Infrastructure
{
    public class AzureClientCredentialHelper : IAzureClientCredentialHelper
    {
        private readonly ImporterConfiguration _configuration;

        public AzureClientCredentialHelper (IOptions<ImporterConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }
        
        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_configuration.Identifier);
         
            return accessToken;
        }
    }
}