using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Application.Contracts;
using Store.Application.DTOs;
using Store.Domain.Models;
using Store.Infrastructure.Data;
using Store.Infrastructure.Migrations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Infrastructure.Implementations
{
	public class UserRepo : IUser
	{
		private readonly AppDbContext appDbContext;
		private readonly IConfiguration configration;

		public UserRepo(AppDbContext appDbContext, IConfiguration configration)
		{
			this.appDbContext = appDbContext;
			this.configration = configration;
		}

		// Login user and generate JWT token
		public async Task<LoginResponse> LoginAsync(LoginDTO loginDTO)
		{
			var getUser = await FindUserByEmail(loginDTO.Email!);
			if (getUser == null) return new LoginResponse(false, "Invalid email or password");

			bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
			if (checkPassword)
			{
				return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
			}
			else
				return new LoginResponse(false, "Invalid email or password");

		}

		// generate JWT token
		private string GenerateJWTToken(ApplicationUser getUser)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var userClaims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, getUser.Id.ToString()),
				new Claim(ClaimTypes.Name, getUser.Name),
				new Claim(ClaimTypes.Email, getUser.Email),
				new Claim(ClaimTypes.Role, getUser.Role),

			};
			var token = new JwtSecurityToken(
				issuer: configration["Jwt:Issuer"],
				audience: configration["Jwt:Audience"],
				claims: userClaims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: credentials
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		// Find user by email
		private async Task<ApplicationUser> FindUserByEmail(string email)
		{
			return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		// Register a new user
		public async Task<RegistResponse> RegisterAsync(RegisterUserDTO registerUserDTO)
		{
			var getUser = await FindUserByEmail(registerUserDTO.Email!);
			if (getUser != null) return new RegistResponse(false, "User with this email already exists");

			appDbContext.Users.Add(new ApplicationUser()
			{
				Email = registerUserDTO.Email,
				Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password),
				Name = registerUserDTO.Name,
			});
			await appDbContext.SaveChangesAsync();
			return new RegistResponse(true, "User registered successfully");
		}
	}
}
