using System.Text.RegularExpressions;

namespace E_ventPlanner.Utils.ServiceUtils;

public static class AuthUtil
{
    public static bool IsValidEmail(string email)
    {
        const string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}