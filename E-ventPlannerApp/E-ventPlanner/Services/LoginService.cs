using System.Diagnostics.Eventing.Reader;
using System.Text.Json.Nodes;
using E_ventPlanner.Contexts;
using E_ventPlanner.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using E_ventPlanner.Services.Interfaces;
using E_ventPlanner.Utils.ServiceUtils;
using Microsoft.AspNetCore.Mvc;

namespace E_ventPlanner.Services;

public class LoginService : ILoginService
{
    private readonly UserManager<IdentityUser> _userManager;

    public LoginService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> LoginUser(LoginDTO user)
    {
        if (!AuthUtil.IsValidEmail(user.Email))
        {
            return false;
        }

        var identityUser = await _userManager.FindByEmailAsync(user.Email);

        if (identityUser == null)
        {
            return false;
        }

        var pwdResult = await _userManager.CheckPasswordAsync(identityUser, user.Password);
        if (!pwdResult)
        {
            return false;
        }

        return true;

    }
}