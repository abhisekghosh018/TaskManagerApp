using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Application.IServices;
using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Services.Member
{
    public class MemberServices : IMemberServices
    {
        private readonly IMember _member;
        private readonly IMapper _mapper;
        public MemberServices(IMember member, IMapper mapper)
        {
            _member = member;
            _mapper = mapper;
        }

        public async Task<CommonReturnDto> CreateMemberAsync(CreateMemberDto dto)
        {
           return await _member.CreateMemberAsync(dto);
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
