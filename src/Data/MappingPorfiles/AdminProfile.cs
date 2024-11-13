using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Data.MappingPorfiles
{
  public class AdminProfile : Profile
  {
    public AdminProfile()
    {
      CreateMap<AdminDto, Admin>()
       .ForMember(dest => dest.Id, opt => opt.Ignore())
       .ReverseMap();
    }
  }
}