using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;

namespace DevTaskTracker.Application.IServices
{
    public interface IMemberServices
    {
        Task<CommonReturnDto> GetAllMembersAsync(int pageNumber);
        Task<CommonReturnDto> CreateNewMemberAsync(CreateMemberDto member);
        Task<CommonReturnDto> GetMembersByIdAsync(Guid id);
        Task<CommonReturnDto> UpdateMemberAsync(UpdateMemberDto member);
        Task<CommonReturnDto> FilterMembers(string? firstName, string? lastName, string? email, int page);
    }
}
