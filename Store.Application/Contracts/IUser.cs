using Store.Application.DTOs;


namespace Store.Application.Contracts
{
	// Interface for managing user authentication and registration in the store application.
	public interface IUser
	{
		Task<RegistResponse> RegisterAsync(RegisterUserDTO registerUserDTO);
		Task<LoginResponse> LoginAsync(LoginDTO loginDTO);
	}
}
