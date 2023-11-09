using Microsoft.AspNetCore.Identity;

namespace E_ventPlanner.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IdentityUser Owner { get; set; }
    public List<IdentityUser> Moderators { get; set; }
    public List<IdentityUser> Users { get; set; }
    public bool IsPriv { get; set; }
}