using System.Threading.Tasks;

namespace SFA.DAS.Courses.Functions.Importer.Domain.Interfaces
{
    public interface IAzureClientCredentialHelper
    {
        Task<string> GetAccessTokenAsync();
    }
}