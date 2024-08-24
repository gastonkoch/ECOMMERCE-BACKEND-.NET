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
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("/UsersAvaible")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<ICollection<UserDto>> GetAllUsersAvaible()
        {
            return Ok(_userService.GetUsersAvaible());
        }

        [HttpGet("{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<UserDto> GetById([FromRoute] int id)
        {
            return Ok(_userService.GetUserById(id));
        }

        [HttpGet("Name/{name}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public ActionResult<UserDto> GetByName([FromRoute] string name)
        {
            return Ok(_userService.GetUserByName(name));
        }

        [HttpPost]
        public ActionResult<UserDto> Create([FromBody] UserCreateRequest user)
        {
            return Ok(_userService.CreateUser(user));
        }

        [HttpPut("ChangeAvailability/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] int id)
        {
            await _userService.ChangeAvailability(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "EveryoneExceptGuests")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateRequest user)
        {
            await _userService.UpdateUser(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<string>> Delete([FromRoute] int id)
        {
            string result = await _userService.DeleteUser(id);
            return Ok(result);
        }

    }
}

