using Microsoft.AspNetCore.Mvc;

namespace Api.AccountTransactions.Authorizations.ApiKeyProtection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyProtectionAttribute : TypeFilterAttribute
    {
        public ApiKeyProtectionAttribute() : base(typeof(ApiKeyProtectionFilter))
        {
        }
    }
}
