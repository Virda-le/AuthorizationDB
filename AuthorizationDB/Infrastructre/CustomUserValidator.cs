using AuthorizationDB.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationDB.Infrastructre
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager,
        AppUser user)
        {
            if (user.Email.ToLower().EndsWith("@example.com"))
                return Task.FromResult(IdentityResult.Success);
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "Only example.com email addresses are allowed"
                }));
            }
        }
    }
}
