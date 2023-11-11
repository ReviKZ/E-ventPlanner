using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using E_ventPlanner.Contexts;
using E_ventPlanner.Enums;
using E_ventPlanner.Models;
using E_ventPlanner.Models.DTOs;
using E_ventPlanner.Services.Interfaces;
using E_ventPlanner.Utils.ServiceUtils;
using Microsoft.AspNetCore.Identity;

namespace E_ventPlanner.Services;

public class RegisterService : IRegisterService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly PlannerDbContext _db;
    public RegisterService(UserManager<IdentityUser> userManager, PlannerDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }
    public async Task<bool> RegisterUser(RegisterDTO user)
    {

        if (!AuthUtil.IsValidEmail(user.Email))
        {
            return false;
        }

        var identityUser = new IdentityUser
        {
            UserName = user.Email,
            Email = user.Email
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (result.Succeeded)
        {
            var createdUser = await _userManager.FindByEmailAsync(user.Email);

            if (createdUser == null)
            {
                return false;
            }

            var userId = createdUser.Id;
            var userData = new UserData
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Role = Role.User
            };
            await _db.UserData.AddAsync(userData);
            await _db.SaveChangesAsync();

            return true;
        }

        return false;
    }
}