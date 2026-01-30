using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SFA.DAS.Courses.Data
{
    public sealed class AzureSqlAccessTokenInterceptor : DbConnectionInterceptor
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly AzureServiceTokenProvider _tokenProvider;

        public AzureSqlAccessTokenInterceptor(AzureServiceTokenProvider tokenProvider)
            => _tokenProvider = tokenProvider;

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            if (connection is SqlConnection sql && string.IsNullOrEmpty(sql.AccessToken))
            {
                sql.AccessToken = await _tokenProvider
                    .GetAccessTokenAsync(AzureResource)
                    .ConfigureAwait(false);
            }

            return result;
        }
    }

}
