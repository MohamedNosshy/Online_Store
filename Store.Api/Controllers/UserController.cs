using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Contracts;
using Store.Application.DTOs;

namespace Store.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUser user;

		public UserController(IUser user)
		{
			this.user = user;
		}
		// api/user/login
		[HttpPost("login")]
		public async Task<ActionResult<LoginResponse>> LoginUser(LoginDTO loginDTO)
		{
			var response = await user.LoginAsync(loginDTO);
			return Ok(response);
		}
		// api/user/register
		[HttpPost("register")]
		public async Task<ActionResult<RegistResponse>> RegisterUser(RegisterUserDTO registerUserDTO)
		{
			var response = await user.RegisterAsync(registerUserDTO);
			return Ok(response);
		}
	}
}
