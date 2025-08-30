using Microsoft.EntityFrameworkCore;
using Store.Domain.Models;


namespace Store.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		
		public DbSet<ApplicationUser> Users { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

	}
}
