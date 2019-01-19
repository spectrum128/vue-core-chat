using AutoMapper;
using VueChat.Dtos;
using VueChat.Entities;

namespace VueChat.Helpers
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