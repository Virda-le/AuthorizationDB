using AuthorizationDB.Infrastructre;
using AuthorizationDB.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var con = "Server = (localdb)\\MSSQLLocalDB;Database = IdentityUsers;Trusted_Connection = True; MultipleActiveResultSets = True";
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(con));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
}
).
    AddEntityFrameworkStores<AppIdentityDbContext>().
    AddDefaultTokenProviders();

//dependency
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidatorWithBase>();
builder.Services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();

builder.Services.AddAuthentication();


builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
