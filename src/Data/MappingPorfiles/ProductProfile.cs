using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Product;

public class ProductProfile : Profile
{
  public ProductProfile()
  {
    // Product mappings
    CreateMap<ProductDto, Product>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Discount mappings
    CreateMap<DiscountDto, Discount>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Variant mappings
    CreateMap<VariantDto, Variant>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Size mappings
    CreateMap<SizeDto, Size>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Color mappings
    CreateMap<ColorDto, Color>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Image mappings
    CreateMap<ImageDto, Image>().ReverseMap();
  }
}
