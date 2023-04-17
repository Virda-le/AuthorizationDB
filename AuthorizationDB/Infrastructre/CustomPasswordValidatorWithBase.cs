using AuthorizationDB.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationDB.Infrastructre
{
    public class CustomPasswordValidatorWithBase : PasswordValidator<AppUser>
    {
        public async override Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager,
        AppUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager, user, password);
            List<IdentityError> errors = result.Succeeded ?
            new List<IdentityError>() : result.Errors.ToList();
            if (password.ToLower().Contains(user.UserName.ToLower()))
                errors.Add(new IdentityError
                {
                    Code = "Password Contains UserName",
                    Description = " Password cannot contain username "
                });
            if (password.Contains("12345"))
                errors.Add(new IdentityError
                {
                    Code = "PasswordContains Sequence",
                    Description = "Password cannot contain numeric sequence"
                });
            return errors.Count == 0 ?
            IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
