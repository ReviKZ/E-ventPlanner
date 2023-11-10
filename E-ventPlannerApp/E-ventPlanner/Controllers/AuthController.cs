using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_ventPlanner.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<bool> Register()
        {
            throw new NotImplementedException();
        }
    }
}
