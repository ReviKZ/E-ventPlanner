using System.Runtime.Serialization;
using E_ventPlanner.Contexts;
using E_ventPlanner.Enums;
using E_ventPlanner.Models;
using E_ventPlanner.Models.DTOs;
using E_ventPlanner.Services.Interfaces;
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
        var identityUser = new IdentityUser
        {
            UserName = user.Email,
            Email = user.Email
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (result.Succeeded)
        {
            var userId = _userManager.Users.First(u => u.Email == user.Email).Id;
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