using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Application.IServices;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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


            var result = await _iuserIdentityService.CreateUserAsync(user, dto.Password, dto.Role);

            //var createResult = await _userManager.CreateAsync(user, dto.Password); // Creating password in the AspNetUsers table
            if (result.UserId == null)
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

            var newMember = _imapper.Map<CreateMemberDto>(member);

            return new CommonReturnDto
            {
                IsSuccess = true,
                ErrorMessage = CommonAlerts.MemberCreateSuccess,
                Data = newMember
            };
        }

        public async Task<CommonReturnDto> GetAllMembersAsync(int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            return await _member.GetAllMembersAsync(pageNumber);
        }

        public async Task<CommonReturnDto> FilterMembers(string? firstName, string? lastName, string? email, int page)
        {
            const int PageSize = 10;

            var memberQuery = _member.GetAllMember(); // Removed Async suffix

            if (!string.IsNullOrEmpty(firstName))
                memberQuery = memberQuery.Where(m => m.FirstName == firstName);

            if (!string.IsNullOrEmpty(lastName))
                memberQuery = memberQuery.Where(m => m.LastName == lastName);

            if (!string.IsNullOrEmpty(email))
                memberQuery = memberQuery.Where(m => m.WorkEmail == email);

            var totalCount = await memberQuery.CountAsync();

            var filteredMembers = await memberQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            if (!filteredMembers.Any())
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.NoMemberFound
                };
            }

            var memberDtos = _imapper.Map<List<GetMembersDto>>(filteredMembers);

            return new CommonReturnDto
            {
                IsSuccess = true,
                Data = memberDtos,
                TotalCount = totalCount // optional
            };
        }


        //public async Task<CommonReturnDto> FilterMembers(string firstName, string lastName, string email, int page)
        //{
        //    if (firstName == null && lastName == null && email == null)
        //    {
        //        return new CommonReturnDto
        //        {
        //            IsSuccess = false,
        //            ErrorMessage = "Enter a search value",

        //        };
        //    }
        //    var memberQuery = _member.GetAllMembersAsync(); // IQueryable<Member>

        //    if (!string.IsNullOrEmpty(firstName))
        //    {
        //        memberQuery = memberQuery.Where(m => m.FirstName == firstName);
        //    }
        //    if (!string.IsNullOrEmpty(lastName))
        //    {
        //        memberQuery = memberQuery.Where(m => m.LastName == lastName);
        //    }
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        memberQuery = memberQuery.Where(m => m.WorkEmail == email);
        //    }

        //    // Pagination logic
        //    int pageSize = 10;
        //    var filteredMembers = await memberQuery
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    // Map to DTOs
        //    var memberDtos = _imapper.Map<List<GetMembersDto>>(filteredMembers);

        //    if (memberDtos == null || memberDtos.Count == 0)
        //    {
        //        return new CommonReturnDto
        //        {
        //            IsSuccess = false,
        //            ErrorMessage = CommonAlerts.NoMemberFound
        //        };
        //    }

        //    return new CommonReturnDto
        //    {
        //        IsSuccess = true,
        //        Data = memberDtos
        //    };

        //    //var membersDtos = _imapper.Map<IList<GetMembersDto>>( _member.GetAllMembersAsync());

        //    //if (membersDtos != null && membersDtos.Count > 0)
        //    //{
        //    //    if (firstName != null)
        //    //    {
        //    //        membersDtos.Where(m => m.FirstName == firstName);
        //    //    }
        //    //    else if(lastName != null) 
        //    //    { 
        //    //        membersDtos.Where(m => m.lastName == lastName); 
        //    //    }
        //    //    else
        //    //    {
        //    //        membersDtos.Where(m => m.WorkEmail == email);
        //    //    }
        //    //}
        //    //return new CommonReturnDto
        //    //{
        //    //    IsSuccess = false,
        //    //    ErrorMessage = CommonAlerts.NoMemberFound,
        //    //};


        //}



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
