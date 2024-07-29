using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        ICollection<UserDto> GetAllUsers();
        UserDto GetUserById(int id);
        Task UpdateUser(int id, UserUpdateRequest user);
        Task<string> DeleteUser(int id);
        UserDto CreateUser(UserCreateRequest user);
        ICollection<UserDto> GetUserByName(string name);
        Task ChangeAvailability(int id);
        ICollection<UserDto> GetUsersAvaible();
    }
}
