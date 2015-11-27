using AngularJSAuthentication.Api.Domain;

namespace AngularJSAuthentication.Api.Repository
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository() : base("AngularJSAuthentication")
        {
        }

        public Account GetByName(string name)
        {
            return ExecuteSingleItem<Account>("select Name, Password from Account where Name = @name", new { name });
        }

        public Account GetByProviderAndProviderKey(string provider, string providerKey)
        {
            return ExecuteSingleItem<Account>("select Name, Password from Account where Provider = @provider and ProviderKey = @providerKey", new { provider, providerKey });
        }
    }
}