

namespace Store.Domain.Models
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Status { get; set; } = "Pending";

		public ApplicationUser? User { get; set; }
		public ICollection<OrderItem>? OrderItems { get; set; }
		

	}
}
