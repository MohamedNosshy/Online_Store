
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Store.Application.Contracts;
using Store.Infrastructure.Data;
using Store.Infrastructure.DependencyInjection;
using Store.Infrastructure.Implementations;


namespace Store.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.InfrastructureServices(builder.Configuration);
			// Resolve JSON Reference Loop Issue
			builder.Services.AddControllers()
			.AddJsonOptions(x =>
			x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			// Enable Swagger with JWT Authentication
			builder.Services.AddSwaggerGen(swagger =>
			{
				swagger.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Store API",
					Description = "Auth With JWT",
				});
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},Array.Empty<string>()

					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();
			// Apply Seeding to fill Admin User if not exists
			SeedAdmin.SeedAdminAsync(app.Services).Wait();

			app.Run();
		}
	}
}
