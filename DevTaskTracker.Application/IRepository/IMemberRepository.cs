using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<CommonReturnDto> GetAllMembersAsync(int pageNumber);
        Task<CommonReturnDto> GetMemberByIdAsync(Guid  id);
        Task<CommonReturnDto> CreateMemberAsync(Member member);
        Task<CommonReturnDto> UpdateMemberAsync(UpdateMemberDto member);
        IQueryable<Member> GetAllMember();
    }
}
