using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DevTaskTracker.Infrastructure.Services
{
    public class MemberServiceRepository : IMemberRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _iMaper;
        private readonly IHttpContextAccessor _iHttpContext;
        private readonly IMemoryCache _imemoryCache;
        public MemberServiceRepository(AppDbContext appDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper iMaper,
            IHttpContextAccessor iHttpContext,
            IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            //_userManager = userManager;
            //_roleManager = roleManager;
            _iMaper = iMaper;
            _iHttpContext = iHttpContext;
            _imemoryCache = memoryCache;
        }

        public async Task<CommonReturnDto> CreateMemberAsync(Member model)
        {
            var result = _appDbContext.Members.Add(model);
            await _appDbContext.SaveChangesAsync();

            return new CommonReturnDto
            {
                IsSuccess = true,
                SuccessMessage = CommonAlerts.MemberCreateSuccess,
                Data = result,
            };
        }

        public async Task<CommonReturnDto> GetAllMembersAsync(int pagenumber)
        {

            string cacheKey = $"member_pages_{pagenumber}";

            if (_imemoryCache.TryGetValue(cacheKey, out List<object>? cachedMembers))
            {

                return new CommonReturnDto
                {
                    Data = cachedMembers!,
                    IsSuccess = true,
                    TotalCount = cachedMembers.Count(),
                };
            }
            int totalCount = await _appDbContext.Members.CountAsync();
            var members = await _appDbContext.Members
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((pagenumber - 1) * 10).Take(10)
                .Select(m => new
                {
                    m.Id,
                    m.FirstName,
                    m.LastName,
                    m.WorkEmail,
                    m.Role,
                    m.Status,    
                    m.IsActive
                }).ToListAsync();
                
               

            if (members == null || members.Count == 0)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound,
                };
            }

            // Optional: Customize cache duration
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            _imemoryCache.Set(cacheKey, members, cacheOptions);

            return new CommonReturnDto
            {
                Data = members,
                IsSuccess = true,
                TotalCount = totalCount,
            };
        }

        public IQueryable<Member> GetAllMember()
        {
            var member = _appDbContext.Members.AsNoTracking();
            return member;   
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
            dto.IsActive = members.IsActive;
            dto.RowVersion = Base64UrlEncoder.Encode( members.RowVersion);

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

            //if (string.IsNullOrEmpty(userRole) || !(userRole == "Admin" || userRole == "OrgAdmin"))
            //{
            //    return new CommonReturnDto
            //    {
            //        IsSuccess = false,
            //        ErrorMessage = CommonAlerts.MemberUpdatedFaild
            //    };
            //}

            

            // 2. Retrieve existing member safely
            var existingMember = await _appDbContext.Members
                                        .FirstOrDefaultAsync(m => m.Id == dto.Id);
            // Check for RowVersion
            var bytesOfRowVersion = Base64UrlEncoder.DecodeBytes(dto.RowVersion);
            _appDbContext.Entry(existingMember).Property(r => r.RowVersion).OriginalValue = bytesOfRowVersion!;

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
            catch (DbUpdateConcurrencyException ex)
            {
                // 5. Optional: Log this with a logger
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to update member. Please try again later." + ex.Message
                };
            }
        }

    }
}
