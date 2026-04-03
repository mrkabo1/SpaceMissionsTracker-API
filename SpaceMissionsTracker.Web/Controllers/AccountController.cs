using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceMissionsTracker.Core.DTOs;
using SpaceMissionsTracker.Core.Identity;
using SpaceMissionsTracker.Core.ServicesContracts;
using System.Security.Claims;

namespace SpaceMissionsTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        public AccountController(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
        private async Task<bool> IsEmailAlreadyExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        /// <summary>
        /// Retrieves a list of all registered users.
        /// </summary>

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.Select(u => new UserResponseDTO
            {
                PersonName = u.PersonName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerDTO.PersonName,
                PhoneNumber = registerDTO.PhoneNumber,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            if (await IsEmailAlreadyExist(registerDTO.Email))
            {
                return BadRequest("The email is already registered.");
            }

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem(ModelState);
            }

            //await _signInManager.SignInAsync(user, isPersistent: false);
            var authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        /// <summary>
        /// Authenticates a user and returns an access token + refresh token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Invalid Model State");
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordCorrect)
            {
                return Unauthorized("Invalid email or password");
            }

            var authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        /// <summary>
        /// Logs out the current user (not implemented by server side).
        /// </summary>

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Refreshes an expired access token using a valid refresh token.
        /// </summary>

        [HttpPost("get-new-token")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }

            ClaimsPrincipal principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);
            if (principal == null)
            {
                return BadRequest("Invalid Jwt Access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return BadRequest("Invalid Refresh token");
            }

            AuthenticationResponseDTO authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            user.RefreshToken = authenticationResponse.RefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }
    }
}
