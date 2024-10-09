using AutoMapper;
using AutoMapper.QueryableExtensions;
using MealOrdering.Server.Data.Context;
using MealOrdering.Server.Data.Models;
using MealOrdering.Server.Services.Infratructure;
using MealOrdering.Shared.DTO;
using MealOrdering.Shared.FilterModels;
using Microsoft.EntityFrameworkCore;

namespace MealOrdering.Server.Services.Services
{
    public class OrderService:IOrderService
    {

        private readonly MealOrderingDbContext context;
        private readonly IMapper mapper;
        private IValidationService validationService;

        public OrderService(MealOrderingDbContext Context, IMapper Mapper, IValidationService ValidationService)
        {
            context = Context;
            mapper = Mapper;
            validationService = ValidationService;
        }


        public async Task<List<OrderDTO>>  GetOrderByFiler(OrderListFilterModel filter)
        {
            var query=context.Orders.Include(i=>i.Supplier).AsQueryable();

            if (filter.CreatedUserId != Guid.Empty)
                query = query.Where(i => i.CreatedUserId == filter.CreatedUserId);

            if (filter.CreateDateFirst.HasValue)
                query = query.Where(i => i.CreateDate >= filter.CreateDateFirst);

            if (filter.CreateDateLast > DateTime.MinValue)
                query = query.Where(i => i.CreateDate <= filter.CreateDateLast);

            var list= await query.ProjectTo<OrderDTO>(mapper.ConfigurationProvider)
                      .OrderBy(i=>i.CreateDate)
                      .ToListAsync();

            return list;
        } 

        public async Task<List<OrderDTO>> GetOrders (DateTime orderDate)
        {
            var list = await context.Orders.Include(i => i.Supplier)
                      .Where(i => i.CreateDate.Date == orderDate.Date)
                      .ProjectTo<OrderDTO>(mapper.ConfigurationProvider)
                      .OrderBy(i => i.CreateDate)
                      .ToListAsync();

            return list;


        }

        public async Task<OrderDTO> GetOrderById(Guid Id)
        {
            return await context.Orders.Where(i => i.Id == Id)
                      .ProjectTo<OrderDTO>(mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync();
        }

        public async Task<OrderDTO> CreateOrder(OrderDTO Order)
        {

            var dbOrder = mapper.Map<Data.Models.Orders>(Order);
            await context.AddAsync(dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderDTO>(dbOrder);
        }

        public async Task<List<OrderDTO>> UpdateOrder (OrderDTO order)
        {
            var dbOrder=await context.Orders.FirstOrDefaultAsync(i=>i.Id==order.Id);

            if(dbOrder == null)
                throw new Exception("Order not found");

            if (!validationService.HasPermission(dbOrder.CreatedUserId))
                throw new Exception("You cannot change the order unless you created");

            mapper.Map(order, dbOrder);
            await context.SaveChangesAsync();
            

            return mapper.Map<List<OrderDTO>>(dbOrder);

        }

        public async Task DeleteOrder(Guid OrderId)
        {
            var detailCount = await context.OrderItems.Where(i => i.OrderId == OrderId).CountAsync();


            if (detailCount > 0)
                throw new Exception($"There are {detailCount} sub items for the order you are trying to delete");

            var order = await context.Orders.FirstOrDefaultAsync(i => i.Id == OrderId);
            if (order == null)
                throw new Exception("Order not found");


            if (!validationService.HasPermission(order.CreatedUserId))
                throw new Exception("You cannot change the order unless you created");



            context.Orders.Remove(order);

            await context.SaveChangesAsync();
        }

        #region OrderItem Methods
        public async Task<List<OrderItemDTO>>GetOrderItems(Guid OrderId)
        {

            return await context.OrderItems.Where(i => i.OrderId == OrderId)
                     .ProjectTo<OrderItemDTO>(mapper.ConfigurationProvider)
                     .OrderBy(i => i.CreateDate)
                     .ToListAsync();
        }
        public async Task<OrderItemDTO> GetOrderItemsById(Guid Id)
        {
            return await context.OrderItems.Include(i => i.Order).Where(i => i.Id == Id)
                      .ProjectTo<OrderItemDTO>(mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync();
        }


        public async Task<OrderItemDTO> CreateOrderItem(OrderItemDTO OrderItem)
        {
            var order = await context.Orders
                .Where(i => i.Id == OrderItem.OrderId)
                .Select(i => i.ExpireDate)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("The main order not found");

            if (order <= DateTime.Now)
                throw new Exception("You cannot create sub order. It is expired !!!");


            var dbOrder = mapper.Map<Data.Models.OrderItems>(OrderItem);
            await context.AddAsync(dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderItemDTO>(dbOrder);
        }

        public async Task<OrderItemDTO> UpdateOrderItem(OrderItemDTO OrderItem)
        {
            var dbOrder = await context.OrderItems.FirstOrDefaultAsync(i => i.Id == OrderItem.Id);
            if (dbOrder == null)
                throw new Exception("Order not found");

            mapper.Map(OrderItem, dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderItemDTO>(dbOrder);
        }

        public async Task DeleteOrderItem(Guid OrderItemId)
        {
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(i => i.Id == OrderItemId);
            if (orderItem == null)
                throw new Exception("Sub order not found");

            context.OrderItems.Remove(orderItem);

            await context.SaveChangesAsync();
        }

       

        public Task<List<OrderDTO>> GetOrdersByFilter(OrderListFilterModel Filter)
        {
            throw new NotImplementedException();
        }

        Task<OrderDTO> IOrderService.UpdateOrder(OrderDTO Order)
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
