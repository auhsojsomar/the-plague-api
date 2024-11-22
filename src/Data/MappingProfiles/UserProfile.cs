using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Data.MappingPorfiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<UserDto, User>()
       .ForMember(dest => dest.Id, opt => opt.Ignore());

      CreateMap<User, UserDto>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
          $"{src.FirstName} {src.MiddleName} {src.LastName}".Trim()));

      CreateMap<UserLoginDto, User>()
       .ForMember(dest => dest.Id, opt => opt.Ignore())
       .ReverseMap();

      CreateMap<GuestDto, UserDto>().ReverseMap();
    }
  }
}