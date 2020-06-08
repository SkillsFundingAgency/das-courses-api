using System.Threading.Tasks;
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
            var clientCredential = new ClientCredential(_configuration.Id, _configuration.Secret);
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{_configuration.Tenant}", true);

            var result = await context.AcquireTokenAsync(_configuration.Identifier, clientCredential).ConfigureAwait(false);

            return result.AccessToken;
        }
    }
}