using DevTaskTracker.Application.DTOs.AuthDtos;
using DevTaskTracker.Application.Services;
using DevTaskTracker.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevTaskTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("register")]       
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return Conflict("A user with this email already exists.");

            var user = new AppUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.UserName,
                EmailConfirmed = true // Optional: auto-confirm email if you're not using email confirmation
            };

            // 1. Create user with password
            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(new
                {
                    Errors = createResult.Errors.Select(e => e.Description)
                });
            }

            var defaultRole = "User";
            //var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
            //if (!roleExists)
            //{
            //    var roleResult = await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            //    if (!roleResult.Succeeded)
            //    {
            //        await _userManager.DeleteAsync(user); // rollback
            //        return StatusCode(500, "Failed to create default role.");
            //    }
            //}

            // Assign default role to the user
            var assignResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!assignResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // rollback
                return StatusCode(500, "Failed to assign default role.");
            }

            // 3. Generate JWT Token
            var token = await _tokenService.CreateToken(user);

            return Ok(new
            {
                message = "User registered successfully.",
                token
            });
        }




        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password)) 
                return Unauthorized("Invalid Credentials");

            return Ok(new {token = await _tokenService.CreateToken(user)});
        }
    }
}
