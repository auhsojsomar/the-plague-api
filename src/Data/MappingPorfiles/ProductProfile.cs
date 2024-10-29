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

    // Forward Mapping: Variant to VariantDto
    CreateMap<Variant, VariantDto>()
        .ForMember(dest => dest.Discount, opt => opt.MapFrom(src =>
            src.Discount != null ? new DiscountDto
            {
              Type = src.Discount.Type,
              Value = src.Discount.Value
            } : null));

    // Reverse Mapping: VariantDto to Variant
    CreateMap<VariantDto, Variant>()
        .ForMember(dest => dest.Discount, opt => opt.MapFrom(src =>
            src.Discount != null ? new Discount
            {
              Type = src.Discount.Type,
              Value = src.Discount.Value
            } : null));

    // Size mappings
    CreateMap<SizeDto, Size>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();

    // Color mappings
    CreateMap<ColorDto, Color>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();
  }
}
