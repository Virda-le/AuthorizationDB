using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace AuthorizationDB.Models;
public class CreateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
public class LoginМodel
{
    [Required]
    [UIHint("email")]
    public string Email { get; set; }
    [Required]
    [UIHint("password")]
    public string Password { get; set; }
}
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
public class RoleEditModel
{
    public IdentityRole Role { get; set; }
    public IEnumerable<AppUser>? Мembers { get; set; }
    public IEnumerable<AppUser>? NonМembers { get; set; }
}
public class RoleМodificationМodel
{
    [Required]
    public string RoleName { get; set; }
    public string RoleId { get; set; }
    public string[]? IdsToAdd { get; set; }
    public string[]? IdsToDelete { get; set; }
}