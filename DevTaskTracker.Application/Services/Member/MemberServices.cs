using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Application.IServices;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Domain.Enums;

namespace DevTaskTracker.Application.Services.Member
{
    public class MemberServices : IMemberServices
    {
        private readonly IMemberRepository _member;
        private readonly IMapper _imapper;
        private readonly IEmailChecker _emailChecker;
        private readonly IUserIdentityService _iuserIdentityService;
        public MemberServices(IMemberRepository member, IMapper mapper,
            IEmailChecker emailChecker, IUserIdentityService userIdentityService)
        {
            _member = member;
            _imapper = mapper;
            _emailChecker = emailChecker;
            _iuserIdentityService = userIdentityService;
        }

        public async Task<CommonReturnDto> CreateNewMemberAsync(CreateMemberDto dto)
        {
            //Check if email already exists
            var IsExisting = await _emailChecker.IsEmailExistsAsync(dto.WorkEmail);
            if (IsExisting == true)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.MemberExistsWithEmail,
                };
            }
            //Create Identity user
           var user = new AppUser
           {
               UserName = dto.WorkEmail,
               Email = dto.WorkEmail,
               FirstName = dto.FirstName,
               LastName = dto.LastName,
               OrganizationId = dto.OrganizationId,
           };
            //MOVED To


            var result = await _iuserIdentityService.CreateUserAsync(user,dto.Password,dto.Role);

            //var createResult = await _userManager.CreateAsync(user, dto.Password); // Creating password in the AspNetUsers table
            if (result.UserId ==null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create Identity user.",
                    Data = result.Errors
                };
            }
            //await _userManager.AddToRoleAsync(user, dto.Role);

            //Map DTO to Entity
            var member = _imapper.Map<DevTaskTracker.Domain.Entities.Member>(dto);
            member.AppUserId = user.Id; // To keep user and member in sync
            member.Status = StatusEnum.Active.ToString();

             await _member.CreateMemberAsync(member);

             var newMember =  _imapper.Map<CreateMemberDto>(member);

            return new CommonReturnDto
            {
                IsSuccess = true,
                ErrorMessage = CommonAlerts.MemberCreateSuccess,
                Data = newMember
            };
        }
         
        public async Task<CommonReturnDto> GetAllMembersAsync()
        {
            return await _member.GetAllMembersAsync();
        }

        public Task<CommonReturnDto> GetMembersByIdAsync(Guid id)
        {
            return _member.GetMemberByIdAsync(id);
        }

        public Task<CommonReturnDto> UpdateMemberAsync(UpdateMemberDto member)
        {
            return _member.UpdateMemberAsync(member);
        }
    }
}
