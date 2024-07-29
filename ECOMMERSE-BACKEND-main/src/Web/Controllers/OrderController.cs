using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Models.Response;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "EveryoneExceptGuests")]
        public ActionResult<OrderWhitPriceResponse> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(_orderService.GetOrderById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "EveryoneExceptGuests")]
        public ActionResult<ICollection<OrderWhitPriceResponse>> GetAll()
        {
            try
            {
                return Ok(_orderService.GetAllOrders());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OrdersByCustomer/{customerId}")]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<ICollection<OrderDto>> GetByCustomerOrders(int customerId)
        {
            try
            {
                return Ok(_orderService.GetOrdersByCustomer(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OrdersBySeller/{sellerId}")]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<ICollection<OrderDto>> GetByCustomerSeller(int sellerId)
        {
            try
            {
                return Ok(_orderService.GetOrdersBySeller(sellerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create/")]
        [Authorize(Policy = "EveryoneExceptGuests")]
        public ActionResult<OrderDto> Create([FromBody] OrderCreateRequest order)
        {
            try
            {
                return _orderService.CreateOrder(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Policy = "CustomerOrSellerPolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ChangeOrderRequest changeOrder)
        {
            try
            {
                await _orderService.UpdateOrderProduct(id, changeOrder);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                string result = await _orderService.DeleteOrder(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }

        [HttpGet("CheckStock/{productId}")]
        [Authorize(Policy = "SellerPolicy")]
        public ActionResult<string> CheckStockProduct([FromRoute] int productId)
        {
            try
            {
                return Ok(_orderService.CheckStockProduct(productId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ChangeStatus/{id}")]
        [Authorize(Policy = "SellerPolicy")]
        public IActionResult ChangeStatus([FromRoute] int id, [FromBody] ChangeStatusOrderRequest changeOrderStatus)
        {
            _orderService.ChangeOrderStatus(id, changeOrderStatus);
            return Ok();
        }
    }
}
