using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface IMember
    {
        Task<CommonReturnDto> GetAllMembersAsync();
        Task<CommonReturnDto> GetMembersByIdAsync(Guid id);
        Task<CommonReturnDto> CreateMemberAsync(CreateMemberDto member);
    }
}
