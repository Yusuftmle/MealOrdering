using MealOrdering.Shared.DTO;
using MealOrdering.Shared.FilterModels;

namespace MealOrdering.Server.Services.Infratructure
{
    public interface IOrderService
    {
        public Task<OrderDTO> CreateOrder(OrderDTO Order);

        public Task<OrderDTO> UpdateOrder(OrderDTO Order);

        public Task DeleteOrder(Guid OrderId);

        public Task<List<OrderDTO>> GetOrders(DateTime OrderDate);

        public Task<List<OrderDTO>> GetOrdersByFilter(OrderListFilterModel Filter);

        public Task<OrderDTO> GetOrderById(Guid Id);
        public Task<OrderItemDTO> CreateOrderItem(OrderItemDTO OrderItem);
        public Task<OrderItemDTO> UpdateOrderItem(OrderItemDTO OrderItem);

        public Task<List<OrderItemDTO>> GetOrderItems(Guid OrderId);

        public Task<OrderItemDTO> GetOrderItemsById(Guid Id);

        public Task DeleteOrderItem(Guid OrderItemId);
    }
}
