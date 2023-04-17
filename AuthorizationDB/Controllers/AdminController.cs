using AuthorizationDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationDB.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordНasher;
        private IUserValidator<AppUser> userValidator;
        public AdminController(UserManager<AppUser> usrMgr,
                               IUserValidator<AppUser> userValid,
                               IPasswordValidator<AppUser> passValid,
                               IPasswordHasher<AppUser> passwordНash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordНasher = passwordНash;

        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
        public ViewResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
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
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string Id, string email, string password)
        {

            AppUser user = await userManager.FindByIdAsync(Id);
            
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                    AddErrorsFromResult(validEmail);
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user,
                   password);
                    if (validPass.Succeeded)
                        user.PasswordHash = this.passwordНasher.HashPassword(user,password); 
                    else
                        AddErrorsFromResult(validPass);
                }
                if ((validEmail.Succeeded && validPass == null)
                || (validEmail.Succeeded && password != string.Empty &&
               validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        AddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            IdentityResult result;
            if(user != null)
                result = await userManager.DeleteAsync(user);                     
            return RedirectToAction("Index");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
