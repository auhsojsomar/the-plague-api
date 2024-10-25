using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Data.MappingPorfiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<UserEmailDto, User>()
       .ForMember(dest => dest.Id, opt => opt.Ignore())
       .ReverseMap();

      CreateMap<UserDto, User>()
       .ForMember(dest => dest.Id, opt => opt.Ignore())
       .ReverseMap();
    }
  }
}