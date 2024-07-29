using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(_productService.GetProductById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Name/{name}")]
        public ActionResult<ICollection<ProductDto>> GetByName([FromRoute] string name)
        {
            try
            {
                return Ok(_productService.GetProductByName(name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/Products")]
        [Authorize]
        [Authorize(Policy = "AdminOrSellerPolicy")]
        public ActionResult<ICollection<Product>> GetAll()
        {
            try
            {
                return Ok(_productService.GetAllProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/ProductsAvaible")]
        public ActionResult<ICollection<Product>> GetAllProductsAvaible()
        {
            try
            {
                return Ok(_productService.GetProductsAvaible());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public ActionResult<ProductDto> Create([FromBody] ProductCreateRequest product)
        {
            try
            {
                return Ok(_productService.CreateProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductChangeRequest product)
        {
            try
            {
                await _productService.UpdateProduct(id, product);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ChangeStock/{id}")]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public async Task<ActionResult<string>> ChangeStock([FromRoute] int id, [FromBody] ChangeStockRequest stock)
        {
            try
            {
                string result = await _productService.ChangeStockProduct(id, stock);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("ChangeAvailability/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminOrSellerPolicy")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] int id)
        {
            try
            {
                await _productService.ChangeAvailability(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<string>> Delete([FromRoute] int id)
        {
            try
            {
                string result = await _productService.DeleteProduct(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
