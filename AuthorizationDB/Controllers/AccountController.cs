using AuthorizationDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace AuthorizationDB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        public AccountController(UserManager<AppUser> userMgr, 
            SignInManager<AppUser>
        signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginМodel details, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(LoginМodel.Email), "Invalid user or password");
            }
                return View(details);
        }
        [Authorize]
        public async  Task<IActionResult> Logout(string returnUrl)
        {
            
             await signInManager.SignOutAsync();
            return Redirect(returnUrl ?? "/");
        }
        [AllowAnonymous]
        public IActionResult Register() => View();
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result
                = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult resultSign =
                    await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (resultSign.Succeeded)
                        return Redirect("/");
                    else
                        ModelState.AddModelError(nameof(LoginМodel.Email), "Invalid user or password");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View(model: "Access denied");
        }
    }
}
