using AutoMapper;
using DevTaskTracker.Application.DTOs.MemberDtos;
using DevTaskTracker.Application.DTOs.TaskDtos;
using DevTaskTracker.Domain.Entities;

namespace DevTaskTracker.Application.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, CreateMemberDto>().ReverseMap();         
            CreateMap<Member, GetMembersDto>().ForMember(dest => dest.Organization, opt=> opt.MapFrom(src=> src.Organization.Name));
            CreateMap<Member, UpdateMemberDto>();
            CreateMap<UpdateMemberDto, Member>().ForMember(dest => dest.Id, opt => opt.Ignore()) // optional
                                                .ForMember(dest => dest.AppUserId, opt => opt.Ignore()); // avoid overwriting system-managed props

            CreateMap<TaskItem, CreateTaskItemDto>().ReverseMap();
            CreateMap<TaskItem, GetTaskDto>().ReverseMap();

            CreateMap<TaskItem, UpdateStatusDto>().ReverseMap();
            CreateMap<UpdateStatusDto,TaskItem>().ReverseMap();

            //CreateMap<AppUser, RegisterDto>().ReverseMap();
        }
    }
}

