using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;

namespace DevTaskTracker.Application.IServices
{
    public interface IMemberServices
    {
        Task<CommonReturnDto> GetAllMembersAsync();
        Task<CommonReturnDto> CreateMemberAsync(CreateMemberDto member);
        Task<CommonReturnDto> GetMembersByIdAsync(Guid id);
    }
}
