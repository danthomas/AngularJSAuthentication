using AngularJSAuthentication.Api.Domain;

namespace AngularJSAuthentication.Api.Repository
{
    public interface IAccountRepository
    {
        Account GetByName(string name);
        Account GetByProviderAndProviderKey(string provider, string providerKey);
    }
}
