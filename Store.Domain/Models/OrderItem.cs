﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Store.Domain.Models
{
	public class OrderItem
	{
		public int Id { get; set; } 
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }

		// Relations
		
		public Order Order { get; set; }
		public Product? Product { get; set; }
	}
}
