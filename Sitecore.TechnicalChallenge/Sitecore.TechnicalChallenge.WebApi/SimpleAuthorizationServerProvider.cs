using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sitecore.TechnicalChallenge.WebApi
{
    public class SimpleAuthorizationServerProvider :OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.Validated(new ClaimsIdentity(context.Options.AuthenticationType));
        }
    }
}