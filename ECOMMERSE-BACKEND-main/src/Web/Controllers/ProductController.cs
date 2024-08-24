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
           return Ok(_productService.GetProductById(id));

        }

        [HttpGet("Name/{name}")]
        public ActionResult<ICollection<ProductDto>> GetByName([FromRoute] string name)
        {
            return Ok(_productService.GetProductByName(name));
        }

        [HttpGet("/Products")]
        [Authorize]
        [Authorize(Policy = "AdminOrSellerPolicy")]
        public ActionResult<ICollection<Product>> GetAll()
        {
            return Ok(_productService.GetAllProducts());
        }

        [HttpGet("/ProductsAvaible")]
        public ActionResult<ICollection<Product>> GetAllProductsAvaible()
        {
             return Ok(_productService.GetProductsAvaible());
        }

        [HttpPost]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public ActionResult<ProductDto> Create([FromBody] ProductCreateRequest product)
        {
            return Ok(_productService.CreateProduct(product));
        }

        [HttpPut("{id}")]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductChangeRequest product)
        {
            await _productService.UpdateProduct(id, product);
            return Ok();
        }

        [HttpPut("ChangeStock/{id}")]
        [Authorize]
        [Authorize(Policy = "SellerPolicy")]
        public async Task<ActionResult<string>> ChangeStock([FromRoute] int id, [FromBody] ChangeStockRequest stock)
        {
            string result = await _productService.ChangeStockProduct(id, stock);
            return Ok(result);
        }


        [HttpPut("ChangeAvailability/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminOrSellerPolicy")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] int id)
        {
            await _productService.ChangeAvailability(id);
            return Ok();
        }


        [HttpDelete("{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<string>> Delete([FromRoute] int id)
        {
            string result = await _productService.DeleteProduct(id);
            return Ok(result);
        }
    }
}
