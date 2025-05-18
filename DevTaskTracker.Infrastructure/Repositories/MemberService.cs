using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Domain.Enums;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevTaskTracker.Infrastructure.Services
{
    public class MemberService : IMember
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _iMaper;
        private readonly IHttpContextAccessor _iHttpContext;
        public MemberService(AppDbContext appDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper iMaper,
            IHttpContextAccessor iHttpContext)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _iMaper = iMaper;
            _iHttpContext = iHttpContext;
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
                OrganizationId = dto.OrganizationId,
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password); // Creating password in the AspNetUsers table
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
            var member = _iMaper.Map<Member>(dto);
            member.AppUserId = user.Id; // To keep user and member in sync
            member.Status = StatusEnum.Active.ToString();

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
                Data = members.ToList(),
                IsSuccess = true
            };
        }

        public async Task<CommonReturnDto> GetMemberByIdAsync(Guid id)
        {
            var members = await _appDbContext.Members.Include(m => m.Organization).FirstOrDefaultAsync(m => m.Id == id);

            if (members == null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound,
                };
            }

            var dto = _iMaper.Map<GetMembersDto>(members);

            return new CommonReturnDto
            {
                Data = dto,
                IsSuccess = true
            };

        }

        public async Task<CommonReturnDto> UpdateMemberAsync(UpdateMemberDto dto)
        {
            // 1. Securely extract current user and role
            var user = _iHttpContext.HttpContext?.User;
            var userRole = user?.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userRole) || !(userRole == "Admin" || userRole == "OrgAdmin"))
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.MemberUpdatedFaild
                };
            }

            // 2. Retrieve existing member safely
            var existingMember = await _appDbContext.Members
                                        .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (existingMember == null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound
                };
            }

            // 3. Map allowed fields only (without overwriting tracked entity)
            _iMaper.Map(dto, existingMember);
            existingMember.UpdatedAt = DateTime.UtcNow;

            // 4. Save changes and return result
            try
            {
                await _appDbContext.SaveChangesAsync();

                return new CommonReturnDto
                {
                    IsSuccess = true,
                    SuccessMessage = CommonAlerts.MemberUpdated,
                    Data = existingMember.Id // Or a safe DTO instead of full model
                };
            }
            catch (DbUpdateException ex)
            {
                // 5. Optional: Log this with a logger
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update member. Please try again later."
                };
            }
        }

    }
}
