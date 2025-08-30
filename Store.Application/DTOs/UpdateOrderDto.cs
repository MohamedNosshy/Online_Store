using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DTOs
{
	// Data Transfer Object for updating order information.
	public class UpdateOrderDto
	{
		public int Id { get; set; }      
		public string Status { get; set; }
	}
}
