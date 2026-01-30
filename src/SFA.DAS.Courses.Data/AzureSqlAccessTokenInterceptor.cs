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

        public override InterceptionResult ConnectionOpening(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result)
        {
            if (connection is SqlConnection sql && string.IsNullOrEmpty(sql.AccessToken))
            {
                // when EF is opening synchronously, set token synchronously, this is
                // used when the CoursesIndexBuilder runs
                sql.AccessToken = _tokenProvider.GetAccessTokenAsync(AzureResource)
                    .GetAwaiter()
                    .GetResult();
            }

            return result;
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            if (connection is SqlConnection sql && string.IsNullOrEmpty(sql.AccessToken))
            {
                // when EF is opening asynchronously, set token asynchronously, this
                // used when requesting data from an endpoint
                sql.AccessToken = await _tokenProvider
                    .GetAccessTokenAsync(AzureResource)
                    .ConfigureAwait(false);
            }

            return result;
        }
    }

}
