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
        private readonly ILoginService _loginService;

        public AuthController(IRegisterService registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<bool> Register(RegisterDTO user)
        {
            return await _registerService.RegisterUser(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<(bool, string)> Login(LoginDTO user)
        {
            return await _loginService.LoginUser(user);
        }
    }
}
