using Store.Application.DTOs;
using Store.Domain.Models;


namespace Store.Application.Contracts
{
	// Interface for managing orders in the store application.
	public interface IOrder
	{
		Task<ServiceResponse> CreateOrderAsync(int userId, Dictionary<int, int> productQuantities);
		Task<ServiceResponse> UpdateAsync(Order order);
		Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
		Task<Order?> GetOrderByIdAsync(int orderId);
		Task<IEnumerable<Order?>> GetAllAsync();
		Task<ServiceResponse> DeleteAsync(int id);
		
	}
}
