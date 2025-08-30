using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DTOs
{
	// Record to represent the response of a registration attempt.
	public record RegistResponse(bool Flag,  string Message = null!);

}
