using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevTaskTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMember _memberService;
        
        public MemberController(IMember memberService)
        {
            _memberService = memberService;
        }

        [Authorize(Roles = "SuperAdmin,OrgAdmin,Admin,User")]
        [HttpGet("getallmembers")]
        public async Task<IActionResult> GetAllMembers()
        {
            var result = await _memberService.GetAllMembersAsync();

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin,Admin,User")]
        [HttpGet("getmemberbyid")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var result = await _memberService.GetMembersByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin,OrgAdmin,Admin")]
        [HttpPost("createmember")]
        public async Task<IActionResult> CreateMember(CreateMemberDto dto)
        {
            var result = await _memberService.CreateMemberAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }

}
