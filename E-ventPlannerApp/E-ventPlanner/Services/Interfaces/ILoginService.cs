using E_ventPlanner.Models.DTOs;

namespace E_ventPlanner.Services.Interfaces;

public interface ILoginService
{
    Task<(bool, string)> LoginUser(LoginDTO user);
}