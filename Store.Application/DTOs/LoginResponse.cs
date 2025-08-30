
namespace Store.Application.DTOs
{
	// Data Transfer Object for user login response.
	public record LoginResponse(bool Flag, string Token = null!, string Message = null!);

}
