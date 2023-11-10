using E_ventPlanner.Enums;

namespace E_ventPlanner.Models;

public class UserData
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
}