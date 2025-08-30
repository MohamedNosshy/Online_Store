

using System.ComponentModel.DataAnnotations;

namespace Store.Application.DTOs
{
	// Data Transfer Object for user registration information.
	public class RegisterUserDTO
	{
		[Required]
		public string? Name { get; set; } = string.Empty;
		[Required, EmailAddress]
		public string? Email { get; set; } = string.Empty;
		[Required]
		public string? Password { get; set; } = string.Empty;
		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string? ConfirmPassword { get; set; } = string.Empty;

	}
}
