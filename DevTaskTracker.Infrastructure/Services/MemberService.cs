using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevTaskTracker.Infrastructure.Services
{
    public class MemberService : IMember
    {

        private readonly AppDbContext _appDbContext;   
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MemberService(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CommonReturnDto> CreateMemberAsync(CreateMemberDto dto)
        {
            // Check if email already exists
            var existing = await _appDbContext.Members
                                .FirstOrDefaultAsync(m => m.WorkEmail == dto.WorkEmail);
            if (existing != null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.MemberExistsWithEmail,
                };
            }
            // Create Identity user
            var user = new AppUser
            {
                UserName = dto.WorkEmail,
                Email = dto.WorkEmail,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                OrganizationId = dto.OrganizationId
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create Identity user.",
                    Data = createResult.Errors
                };
            }
            await _userManager.AddToRoleAsync(user, dto.Role);

            // Map DTO to Entity
            var member = new Member
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName ,
                LastName = dto.LastName,
                WorkEmail = dto.WorkEmail,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IP = dto.IP,
                GitRepo = dto.GitRepo,
                OrganizationId = dto.OrganizationId
            };

            // Save to database
            _appDbContext.Members.Add(member);
            await _appDbContext.SaveChangesAsync();

            return new CommonReturnDto
            {
                IsSuccess = true,
                SuccessMessage = CommonAlerts.MemberCreateSuccess,
                Data = member.Id
            };
        }

        public async Task<CommonReturnDto> GetAllMembersAsync()
        {
            var members = await _appDbContext.Members.ToListAsync();
            if (members == null || members.Count == 0)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound,
                };
            }   
            return new CommonReturnDto
            {
                Data = await _appDbContext.Members.ToListAsync(),
                IsSuccess = true
            };
        }

        public async Task<CommonReturnDto> GetMembersByIdAsync(string id)
        {
            var members = await _appDbContext.Members.FirstOrDefaultAsync(m=> m.Id == id);

            if (members == null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound,
                };
            }
            return new CommonReturnDto
            {
                Data = members,
                IsSuccess = true
            };

        }
    }
}
