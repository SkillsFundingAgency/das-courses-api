using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SFA.DAS.Courses.Api.Infrastructure
{
    public class LocalDataLoadAuthenticationHandler : AuthenticationHandler<LocalDataLoadAuthenticationSchemeOptions>
    {
        
        public LocalDataLoadAuthenticationHandler(IOptionsMonitor<LocalDataLoadAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, RoleNames.DataLoad) 
            };
            var identity = new ClaimsIdentity(claims, "Local");
            var principal = new ClaimsPrincipal(identity);
            
            
            var ticket = new AuthenticationTicket(principal, "Local");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}