using AutoMapper;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DevTaskTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberServices _memberService;
        private readonly IMapper _imapper;

        public MemberController(IMemberServices memberService, IMapper imapper)
        {
            _memberService = memberService;
            _imapper = imapper;
        }

       // [Authorize(Roles = "SuperAdmin,OrgAdmin,Admin")]
        [HttpGet("getallmembers")]
        public async Task<IActionResult> GetAllMembers(int pageNumber)
        {
            var result = await _memberService.GetAllMembersAsync(pageNumber);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        //[Authorize(Roles = "SuperAdmin,OrgAdmin,Admin,User")]
        [HttpGet("getmemberbyid")]
        public async Task<IActionResult> GetMemberById(  Guid id)
        {
            var result = await _memberService.GetMembersByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[Authorize(Roles = "SuperAdmin,OrgAdmin,Admin")]
        [HttpPost("createmember")]
        public async Task<IActionResult> CreateMember(CreateMemberDto createMemberDto)
        {
            if (createMemberDto == null)
            {
                return BadRequest("Request data is null.");
            }

             var  memberImageUrl= await _memberService.MemberFileImageUoloadAsync(createMemberDto.File!);

            createMemberDto.ImageUrl = memberImageUrl?.Data?.ToString();
            var result = await _memberService.CreateNewMemberAsync(createMemberDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //[Authorize(Roles = "SuperAdmin,OrgAdmin,Admin")]
        [HttpPut("updatemember")]
        public async Task<IActionResult> UpdateMember(UpdateMemberDto updateMemberDto)
        {
            if (updateMemberDto == null)
            {
                return BadRequest("Request data is null.");
            }

            var result = await _memberService.UpdateMemberAsync(updateMemberDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("filterMember")]
        public async Task<IActionResult> FilterMember(string? firstName, string? lastName, string? email, int page)
        {
           var filteredMember= await _memberService.FilterMembers(firstName, lastName, email, page);

            if(filteredMember.IsSuccess)
            {
                return Ok(filteredMember);
                
            }
            return BadRequest(filteredMember);
        }
        
    }

}
