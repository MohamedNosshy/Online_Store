using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DTOs
{
	// Data Transfer Object for user login information.
	public class LoginDTO
	{
		[Required, EmailAddress]
		public string? Email { get; set; } = string.Empty;
		[Required]
		public string? Password { get; set; } = string.Empty;
	}
}
