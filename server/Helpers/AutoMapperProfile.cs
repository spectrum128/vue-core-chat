using AutoMapper;
using StChat.Dtos;
using StChat.Entities;

namespace StChat.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, ContactDto>();
            CreateMap<ContactRequest, ContactRequestDto>();
            CreateMap<ContactRequestDto, ContactRequest>();

        }
    }
}