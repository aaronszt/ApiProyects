using ApiProyects.Models;
using ApiProyects.Models.Dtos;
using AutoMapper;

namespace ApiProyects.Mappers
{
    public class MappingCfg : Profile
    {
        public MappingCfg()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}
