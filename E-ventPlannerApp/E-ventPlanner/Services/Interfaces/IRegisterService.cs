using E_ventPlanner.Models.DTOs;

namespace E_ventPlanner.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<bool> RegisterUser(RegisterDTO user);
    }
}