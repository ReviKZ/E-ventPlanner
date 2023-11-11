using E_ventPlanner.Models.DTOs;
using E_ventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_ventPlanner.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public AuthController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<bool> Register(RegisterDTO user)
        {
            return await _registerService.RegisterUser(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<bool> Login(LoginDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
