using Microsoft.Extensions.DependencyInjection;
using Store.Domain.Models;
using Store.Infrastructure.Data;

public static class SeedAdmin
{
	public static async Task SeedAdminAsync(IServiceProvider services)
	{

		// Apply Seeding to fill Admin User if not exists
		using var scope = services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (!context.Users.Any(u => u.Email == "admin@local"))
		{
			var admin = new ApplicationUser
			{
				Name = "ADMIN",
				Email = "admin@local",
				Password = BCrypt.Net.BCrypt.HashPassword("Admin@123456"),
				Role = "Admin"
			};
			context.Users.AddAsync(admin);
			await context.SaveChangesAsync();
		}
	}
}
