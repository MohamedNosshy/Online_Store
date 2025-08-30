

using Store.Application.DTOs;
using Store.Domain.Models;

namespace Store.Application.Contracts
{
	// Interface for managing products in the store application.
	public interface IProduct
	{
		Task<ServiceResponse> AddAysync(Product product);
		Task<ServiceResponse> UpdateAsync(Product product);
		Task<ServiceResponse> DeleteAsync(int id);
		Task<IEnumerable<Product>> GetAllAsync();
		Task<Product?> GetByIdAsync(int id);
		
	}
}
