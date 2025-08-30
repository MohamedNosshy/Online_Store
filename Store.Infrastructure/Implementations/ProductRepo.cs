
using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts;
using Store.Application.DTOs;
using Store.Domain.Models;
using Store.Infrastructure.Data;

namespace Store.Infrastructure.Implementations
{
	public class ProductRepo : IProduct
	{
		private readonly AppDbContext appDbContext;

		public ProductRepo(AppDbContext appDbContext)
		{
			this.appDbContext = appDbContext;
		}

		// Add a new product
		public async Task<ServiceResponse> AddAysync(Product product)
		{
			appDbContext.Products.Add(product);
			await SaveChangesAsync();
			return new ServiceResponse(true, "Product Added successfully");
		}

		// Delete product by id
		public async Task<ServiceResponse> DeleteAsync(int id)
		{
			var product = await appDbContext.Products.FindAsync(id);
			if (product == null)
			{
				return new ServiceResponse(false, "Product not found");
			}
			appDbContext.Products.Remove(product);
			await SaveChangesAsync();
			return new ServiceResponse(true, "Product deleted successfully");
		}

		// Get all products
		public async Task<IEnumerable<Product>> GetAllAsync() => await appDbContext.Products.AsNoTracking().ToListAsync();

		// Get product by id
		public async Task<Product> GetByIdAsync(int id) => await appDbContext.Products.FindAsync(id);

		// Update an existing product
		public async Task<ServiceResponse> UpdateAsync(Product product)
		{
			appDbContext.Update(product);
			await SaveChangesAsync();
			return new ServiceResponse(true, "Product updated successfully");
		}

		// Save changes to the database
		private async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();

	}
}
