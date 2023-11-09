using Microsoft.AspNetCore.Identity;

namespace E_ventPlanner.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Description { get; set; }
    public IdentityUser Owner { get; set; }
    public List<IdentityUser> Moderators { get; set; }
    public List<IdentityUser> Invited { get; set; }

}