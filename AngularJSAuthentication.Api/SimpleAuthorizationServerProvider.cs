using System.Security.Claims;
using System.Threading.Tasks;
using AngularJSAuthentication.Api.Domain;
using AngularJSAuthentication.Api.Repository;
using Microsoft.Owin.Security.OAuth;

namespace AngularJSAuthentication.Api
{
    internal class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAccountRepository _accountRepository;

        public SimpleAuthorizationServerProvider(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            Account account = _accountRepository.GetByName(context.UserName);

            if (account == null || account.Password != context.Password)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", "sdfasdfasdf"));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}