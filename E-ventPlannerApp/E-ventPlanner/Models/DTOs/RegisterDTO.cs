using E_ventPlanner.Enums;

namespace E_ventPlanner.Models.DTOs;

public class RegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
}