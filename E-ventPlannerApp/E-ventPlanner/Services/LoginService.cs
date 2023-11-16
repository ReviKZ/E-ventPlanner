using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_ventPlanner.Contexts;
using E_ventPlanner.Enums;
using E_ventPlanner.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using E_ventPlanner.Services.Interfaces;
using E_ventPlanner.Utils.ServiceUtils;
using Microsoft.IdentityModel.Tokens;

namespace E_ventPlanner.Services;

public class LoginService : ILoginService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly PlannerDbContext _db;
    private readonly IConfiguration _config;

    public LoginService(UserManager<IdentityUser> userManager, PlannerDbContext database, IConfiguration config)
    {
        _userManager = userManager;
        _db = database;
        _config = config;
    }

    public async Task<(bool, string)> LoginUser(LoginDTO user)
    {
        if (!AuthUtil.IsValidEmail(user.Email))
        {
            return (false, "This is a wrong e-mail format!");
        }

        var identityUser = await _userManager.FindByEmailAsync(user.Email);

        if (identityUser == null)
        {
            return (false, "There isn't a user with this email");
        }

        var pwdResult = await _userManager.CheckPasswordAsync(identityUser, user.Password);
        if (!pwdResult)
        {
            return (false, "Wrong email-password combination");
        }

        string token = GenerateToken(user);
        return (true, token);

    }

    private string GenerateToken(LoginDTO user)
    {
        var identityUser = _userManager.FindByEmailAsync(user.Email).Result;
        var userData = _db.UserData.FirstOrDefault(u => u.UserId == identityUser.Id);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email ,user.Email),
            new Claim(ClaimTypes.Role, ((Role)userData.Role).ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
        SigningCredentials creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims:claims,
            expires:DateTime.Now.AddMinutes(120),
            issuer:_config.GetSection("Jwt:Issuer").Value,
            audience:_config.GetSection("Jwt:Audience").Value,
            signingCredentials: creds
        );
        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }
}