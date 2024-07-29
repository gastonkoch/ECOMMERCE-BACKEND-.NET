using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<ICollection<User>> GetAll()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/UsersAvaible")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<ICollection<UserDto>> GetAllUsersAvaible()
        {
            try
            {
                return Ok(_userService.GetUsersAvaible());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<UserDto> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(_userService.GetUserById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Name/{name}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<UserDto> GetByName([FromRoute] string name)
        {
            try
            {
                return Ok(_userService.GetUserByName(name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<UserDto> Create([FromBody] UserCreateRequest user)
        {
            try
            {
                return Ok(_userService.CreateUser(user));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ChangeAvailability/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] int id)
        {
            try
            {
                await _userService.ChangeAvailability(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "EveryoneExceptGuests")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateRequest user)
        {
            try
            {
                await _userService.UpdateUser(id, user);
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
                string result = await _userService.DeleteUser(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

