using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Contracts;
using Store.Application.DTOs;
using Store.Domain.Models;
using Store.Infrastructure.Implementations;
using System.Security.Claims;

namespace Store.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrder order;

		public OrderController(IOrder order)
		{
			this.order = order;
		}

		// Get all orders - Admin only
		[HttpGet]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> GetAllAsync()
		{
			var orders = await order.GetAllAsync();
			return Ok(orders);
		}

		// Update order status - Admin only
		[HttpPut]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> UpdateAsync([FromBody] UpdateOrderDto orderDto)
		{
			var allowedStatuses = new[] { "Pending", "Paid" };

			if (!allowedStatuses.Contains(orderDto.Status))
			{
				return BadRequest(new { Message = "Invalid status. Allowed values: " + string.Join(", ", allowedStatuses) });
			}

			var orderUpdated = new Order
			{
				Id = orderDto.Id,
				Status = orderDto.Status
			};

			var response = await order.UpdateAsync(orderUpdated);

			if (!response.Flag)
			{
				return NotFound(response);
			}

			return Ok(response);
		}

		// Delete order - Admin only
		[HttpDelete("{id}")]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var response = await order.DeleteAsync(id);
			return Ok(response);
		}

		// Get order by ID - Admin only
		[HttpGet("id")]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var ord = await order.GetOrderByIdAsync(id);
			if (ord == null)
			{
				return NotFound(new { Message = "Order not found" });
			}
			return Ok(ord);
		}

		// Get orders for the logged-in user
		[HttpGet("my-orders")]
		[Authorize(Policy = "UserOrAdmin")]
		public async Task<IActionResult> GetMyOrdersAsync()
		{

			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

			var orders = await order.GetOrdersByUserIdAsync(userId);
			return Ok(orders);
		}

		// Create a new order for a user
		[HttpPost("create/{userId}")]
		[Authorize(Policy = "UserOrAdmin")]
		public async Task<IActionResult> CreateOrderAsync(int userId, [FromBody] Dictionary<int, int> productQuantities)
		{
			var response = await order.CreateOrderAsync(userId, productQuantities);
			if (!response.Flag)
			{
				return BadRequest(new { response.Message });
			}
			return Ok(response);
		}





	}
}
