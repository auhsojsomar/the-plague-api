using AutoMapper;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;

public class OrdeProfile : Profile
{
  public OrdeProfile()
  {
    // Map OrderStatus to BaseDto
    CreateMap<OrderStatus, BaseDto>();

    // Map PaymentMethod to BaseDto
    CreateMap<PaymentMethod, BaseDto>();

    // Map PaymentStatus to BaseDto
    CreateMap<PaymentStatus, BaseDto>();

    // Map ShippingFee to BaseDto
    CreateMap<ShippingFee, ShippingFeeDto>();

    CreateMap<OrderItem, OrderItemDto>();

    // Optionally, map Order to OrderDto (if needed)
    CreateMap<Order, OrderDto>()
    .ForMember(dest => dest.OrderStatus, opt => opt.Ignore()) // To be mapped separately
    .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore()) // To be mapped separately
    .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore()) // To be mapped separately
    .ForMember(dest => dest.ShippingFee, opt => opt.Ignore()) // To be mapped separately
    ;
  }
}
