using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Store.Application.Contracts;
using Store.Infrastructure.Data;
using Store.Infrastructure.Implementations;
using System.Security.Claims;
using System.Text;

namespace Store.Infrastructure.DependencyInjection
{
	// Static class for configuring and registering infrastructure services.
	public static class ServiceContainer
	{
		public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Configure the database context with SQL Server and set the migration assembly.
			services.AddDbContext<AppDbContext>(
				options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName)),
				ServiceLifetime.Scoped);

			// Configure JWT authentication.
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
					RoleClaimType = ClaimTypes.Role
				};
			});
			// Configure authorization policies.
			services.AddAuthorization( options =>
			{
				options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
				options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
				options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
			});
			// Register repository implementations for dependency injection.
			services.AddScoped<IUser,UserRepo>();
			services.AddScoped<IOrder, OrderRepo>();
			services.AddScoped<IProduct, ProductRepo>();

			return services;
		}
	}
}
