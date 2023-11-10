using Microsoft.AspNetCore.Authorization;

namespace ResoucesWebApi.Policies
{
    public class RolePolicies
    {
        public const string Admin = "Admin";
        public const string Accountant = "Accountant";
        public const string Seller = "Seller";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Admin)
                .Build();
        }

        public static AuthorizationPolicy AccountantPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Accountant, Admin)
                .Build();
        }

        public static AuthorizationPolicy SellerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Seller, Admin)
                .Build();
        }
    }
}
