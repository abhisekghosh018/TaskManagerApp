using DevTaskTracker.Application.DTOs.AuthDtos;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevTaskTracker.Infrastructure.Services
{
    public class AuthService : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }

        public async Task<CommonReturnDto> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return new CommonReturnDto
                {
                    AlreadyExistMessage = CommonAlerts.EmailAlreadyExists,
                    IsSuccess = false
                };
            // Create the Organization object first
            var organization = new Organization
            {
                Name = dto.OrganizationName,                   
            };

            // Create the AppUser object
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,                              
                PhoneNumber = dto.Contact,
                Organization = organization, // Link the organization
                EmailConfirmed = true
            };
            // Check Create the Organization if exists
            var existingOrg = await _appDbContext.Set<Organization>()
            .FirstOrDefaultAsync(o => o.Name == dto.OrganizationName);

            if (existingOrg != null)
            {
                return new CommonReturnDto
                {
                    AlreadyExistMessage = CommonAlerts.OrganizationExists,
                    IsSuccess = false
                };
            }
            else
            {
                // Create the user with password
                var createResult = await _userManager.CreateAsync(user, dto.Password);
                if (!createResult.Succeeded)
                {
                    return new CommonReturnDto
                    {
                        ErrorMessage = createResult.Errors.ToString(),
                        IsSuccess = false
                    };
                }
            }

            // Ensure default 'User' role exists
            var defaultRole = "OrgAdmin";
            var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user); // rollback
                    //return StatusCode(500, "Failed to create default role.");
                    return new CommonReturnDto
                    {
                        ErrorMessage = CommonAlerts.RoleAssignmentFailed,
                        IsSuccess = false
                    };
                }
            }

            // Assign role to the new user
            var assignResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!assignResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // rollback               
                return new CommonReturnDto
                {
                    ErrorMessage = CommonAlerts.RoleAssignmentFailed,
                    IsSuccess = false
                };
            }

            // Generate JWT token   
            var token = await _tokenService.CreateToken(user);

            return new CommonReturnDto
            {
                Token = token,
                SuccessMessage = CommonAlerts.UserRegisteredSuccessfully,
                IsSuccess = true
            };

        }

        public async Task<CommonReturnDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new CommonReturnDto
                {
                    ErrorMessage = CommonAlerts.LoginFailed,
                    IsSuccess = false
                };

            return new CommonReturnDto
            {
                Token = await _tokenService.CreateToken(user),
            };
        }

    }

}
