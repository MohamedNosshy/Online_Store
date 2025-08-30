using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts;
using Store.Application.DTOs;
using Store.Domain.Models;
using Store.Infrastructure.Data;



namespace Store.Infrastructure.Implementations
{
	public class OrderRepo : IOrder
	{
		private readonly AppDbContext appDbContext;
		public OrderRepo(AppDbContext appDbContext)
		{
			this.appDbContext = appDbContext;
		}

		// productQuantities: Dictionary<ProductId, Quantity>
		public async Task<ServiceResponse> CreateOrderAsync(int userId, Dictionary<int, int> productQuantities)
		{
			var user = await appDbContext.Users.FindAsync(userId);
			if (user == null)
			{
				return new ServiceResponse(false, "User not found");
			}

			var order = new Order
			{
				UserId = userId,
				CreatedAt = DateTime.UtcNow,
				Status = "Pending",
				OrderItems = new List<OrderItem>()
			};

			foreach (var pq in productQuantities)
			{
				var product = await appDbContext.Products.FindAsync(pq.Key);
				if (product == null)
					return new ServiceResponse(false, $"Product with Id {pq.Key} not found");

				order.OrderItems.Add(new OrderItem
				{
					ProductId = pq.Key,
					Quantity = pq.Value,
					UnitPrice = product.Price * pq.Value
				});
			}

			appDbContext.Orders.Add(order);
			await SaveChangesAsync();

			return new ServiceResponse(true, "Order created successfully");
		}

		// Delete order by id
		public async Task<ServiceResponse> DeleteAsync(int id)
		{
			var order = await appDbContext.Orders.FindAsync(id);
			if (order == null)
				return new ServiceResponse(false, "Order not found");

			appDbContext.Orders.Remove(order);
			await SaveChangesAsync();

			return new ServiceResponse(true, "Order deleted successfully");
		}

		// Get all orders with their items and associated products
		public async Task<IEnumerable<Order?>> GetAllAsync()
		{
			return await appDbContext.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.AsNoTracking()
				.ToListAsync();
		}

		// Get a specific order by its ID, including its items and associated products
		public async Task<Order?> GetOrderByIdAsync(int orderId)
		{
			return await appDbContext.Orders
			   .Include(o => o.OrderItems)
			   .ThenInclude(oi => oi.Product)
			   .FirstOrDefaultAsync(o => o.Id == orderId);
		}

		// get all orders for a specific user by their user ID
		public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
		{
			return await appDbContext.Orders
			   .Include(o => o.OrderItems)
			   .ThenInclude(oi => oi.Product)
			   .Where(o => o.UserId == userId)
			   .AsNoTracking()
			   .ToListAsync();
		}

		// Update an existing order's status
		public async Task<ServiceResponse> UpdateAsync(Order order)
		{
			var existingOrder = await appDbContext.Orders.FindAsync(order.Id);
			if (existingOrder == null)
			{
				return new ServiceResponse(false, $"Order with ID {order.Id} not found.");
			}

			existingOrder.Status = order.Status;

			appDbContext.Orders.Update(existingOrder);
			await appDbContext.SaveChangesAsync();

			return new ServiceResponse(true, "Order updated successfully");
		}

		// Save changes to the database
		private async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();
	}

}
