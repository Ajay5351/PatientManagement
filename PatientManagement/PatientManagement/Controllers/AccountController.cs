using Microsoft.AspNetCore.Mvc;
using PatientManagement.BusinessLogic;
using PatientManagement.Models;

namespace PatientManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupModel signupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountRepository.SignupAsync(signupModel);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    Message = "User registered successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "User registration failed",
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _accountRepository.LoginAsync(signInModel);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new
                {
                    Message = "Invalid email or password"
                });
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
