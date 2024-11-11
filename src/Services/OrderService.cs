using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using static The_Plague_Api.Helpers.ValidationHelpers;
using static The_Plague_Api.Helpers.StockHelpers;

namespace The_Plague_Api.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentStatusRepository _paymentStatusRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly ICartService _cartService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPaymentStatusRepository paymentStatusRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IOrderStatusRepository orderStatusRepository,
        ICartService cartService)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _paymentStatusRepository = paymentStatusRepository;
      _paymentMethodRepository = paymentMethodRepository;
      _orderStatusRepository = orderStatusRepository;
      _cartService = cartService;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
      return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
      ValidateId(id);
      return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
      await ValidateEntityIdsAsync(order);
      await UpdateStockAndRemoveFromCartOnPayment(order);
      return await _orderRepository.CreateAsync(order);
    }

    public async Task<bool> UpdateOrderAsync(string id, Order order)
    {
      ValidateId(id);
      await ValidateEntityIdsAsync(order);
      await UpdateStockAndRemoveFromCartOnPayment(order);
      return await _orderRepository.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
      ValidateId(id);
      return await _orderRepository.DeleteAsync(id);
    }

    private async Task ValidateEntityIdsAsync(Order order)
    {
      foreach (var item in order.Items)
      {
        await ValidateProductAndVariantAsync(item.ProductId, item.VariantId, _productRepository);
      }
      await ValidateEntityExistenceByKeyAsync(order.OrderStatusKey, _orderStatusRepository, "OrderStatus");
      await ValidateEntityExistenceByKeyAsync(order.PaymentMethodKey, _paymentMethodRepository, "PaymentMethod");
      await ValidateEntityExistenceByKeyAsync(order.PaymentStatusKey, _paymentStatusRepository, "PaymentStatus");
    }

    private async Task UpdateStockAndRemoveFromCartOnPayment(Order order)
    {
      if (order.PaymentStatusKey == 2) // Assuming 2 is the 'Paid' status
      {
        foreach (var item in order.Items)
        {
          await UpdateVariantQuantityAsync(item.ProductId, item.VariantId, item.Quantity, _productRepository);
        }

        // Remove only the ordered items from the user's cart
        if (order.UserId != null) await _cartService.RemoveOrderedItemsFromCartAsync(order.UserId, order.Items);
      }
    }
  }
}
