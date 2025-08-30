using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Contracts;
using Store.Application.DTOs;
using Store.Domain.Models;


namespace Store.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProduct product;
		public ProductController(IProduct product)
		{
			this.product = product;
		}

		// Get all products - Admin and User
		[HttpGet]
		[Authorize(Policy = "UserOrAdmin")]
		public async Task<IActionResult> GetAllAsync()
		{
			var products = await product.GetAllAsync();
			return Ok(products);
		}

		// Get product by ID - Admin only
		[HttpGet("{id}")]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var prod = await product.GetByIdAsync(id);
			if (prod == null)
			{
				return NotFound(new { Message = "Product not found" });
			}
			return Ok(prod);
		}

		// Add new product - Admin only
		[HttpPost]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> AddAsync([FromBody] ProductDTO productModel)
		{

			var newproduct = new Product
			{
				Id = productModel.Id,
				Name = productModel.Name,
				Price = productModel.Price,
				Stock = productModel.Stock
			};
			var response = await product.AddAysync(newproduct);
			return Ok(response);
		}

		// Update product - Admin only
		[HttpPut]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> UpdateAsync([FromBody] ProductDTO productModel)
		{
			var newproduct = new Product
			{
				Id = productModel.Id,
				Name = productModel.Name,
				Price = productModel.Price,
				Stock = productModel.Stock
			};
			var response = await product.UpdateAsync(newproduct);
			return Ok(response);
		}

		// Delete product - Admin only
		[HttpDelete("{id}")]
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> DeleteAsync(int id)
		{

			var response = await product.DeleteAsync(id);
			return Ok(response);
		}


	}
}
