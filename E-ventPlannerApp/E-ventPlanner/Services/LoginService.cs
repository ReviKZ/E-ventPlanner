using System.Diagnostics.Eventing.Reader;
using E_ventPlanner.Contexts;
using E_ventPlanner.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using E_ventPlanner.Services.Interfaces;

namespace E_ventPlanner.Services;

public class LoginService : ILoginService
{
    private readonly UserManager<IdentityUser> _userManager;

    public LoginService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(bool, string)> LoginUser(LoginDTO user)
    {
        if (!IsValidEmail(user.Email))
        {
            return (false, "Not valid e-mail format!");
        }

        var identityUser = await _userManager.FindByEmailAsync(user.Email);

        if (identityUser == null)
        {
            return (false, "User with this e-mail was not found!");
        }

        var pwdResult = await _userManager.CheckPasswordAsync(identityUser, user.Password);
        if (!pwdResult)
        {
            return (false, "The password was not correct!");
        }

        return (true, identityUser.Id);

    }

    // Helper method to check if the email has a valid format
    private bool IsValidEmail(string email)
    {
        const string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}