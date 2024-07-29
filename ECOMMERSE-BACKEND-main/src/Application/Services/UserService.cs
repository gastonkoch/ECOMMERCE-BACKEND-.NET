using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public UserService(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public ICollection<UserDto> GetAllUsers()
        {
            var users = UserDto.ToList(_userRepository.ListAsync().Result ?? throw new Exception("No se encontraron usuarios"));
            return users;
        }

        public UserDto GetUserById(int id)
        {
            UserDto userDto = UserDto.ToDto(_userRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el usuario"));
            return userDto;
        }

        public ICollection<UserDto> GetUserByName(string name)
        {
            List<UserDto> users = UserDto.ToList(_userRepository.GetByName(name).Result ?? throw new Exception("No se encontro el producto"));
            return users;
        }

        public UserDto CreateUser(UserCreateRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Ingrese el nombre del usuario", nameof(user.Name));
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ArgumentException("Ingrese el apellido del usuario", nameof(user.LastName));
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Ingrese una contraseña", nameof(user.Password));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Ingrese una email", nameof(user.Email));
            }

            if (!Enum.IsDefined(typeof(UserType), user.UserType))
            {
                throw new ArgumentException("El tipo de usuario es inválido", nameof(user.UserType));
            }

            var userInDataBase = _userRepository.GetByUserEmail(user.Email);
            if (userInDataBase is not null)
            {
                throw new ArgumentException($"El mail {user.Email} ya se encuentra en uso");
            }

            UserDto userCreate = new UserDto();
            userCreate.Name = user.Name;
            userCreate.LastName = user.LastName;
            userCreate.Email = user.Email;
            userCreate.Password = user.Password;
            userCreate.UserType = user.UserType;
            userCreate.Avaible = true;
            userCreate.RegisterDate = DateOnly.FromDateTime(DateTime.Now);
            return UserDto.ToDto(_userRepository.AddAsync(UserDto.ToEntity(userCreate)).Result);
        }

        public async Task UpdateUser(int id, UserUpdateRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Ingrese el nombre del usuario", nameof(user.Name));
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ArgumentException("Ingrese el apellido del usuario", nameof(user.LastName));
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Ingrese una contraseña", nameof(user.Password));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Ingrese una email", nameof(user.Email));
            }

            if (!Enum.IsDefined(typeof(UserType), user.UserType))
            {
                throw new ArgumentException("El tipo de usuario es inválido", nameof(user.UserType));
            }

            var userValidate = _userRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el usuario");
            
            userValidate.Name = user.Name;
            userValidate.Email = user.Email;
            userValidate.Password = user.Password;
            userValidate.LastName = user.LastName;
            userValidate.UserType = user.UserType;

            await _userRepository.UpdateAsync(userValidate);
        }
        public async Task ChangeAvailability(int id)
        {
            var user= _userRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el usuario");

            user.Avaible = !user.Avaible;

            await _userRepository.UpdateAsync(user);
        }

        public ICollection<UserDto> GetUsersAvaible()
        {
            var users = UserDto.ToList(_userRepository.GetListAvaible().Result ?? throw new Exception("No se encontraron usuarios"));
            return users;
        }

        public async Task<string> DeleteUser(int id)
        {
            var userDto = _userRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el usuario");

            if (userDto.UserType == UserType.Admin)
            {
                throw new Exception("ERROR: No es posible la eliminacion del usuario debido a que es el admin");
            }

            List<OrderDto> orderCustomer = OrderDto.ToList(_orderRepository.OrdersByCustomer(id).Result);
            List<OrderDto> orderSeller = OrderDto.ToList(_orderRepository.OrdersBySellers(id).Result);
            
            if (orderCustomer.Count > 0 || orderSeller.Count > 0) 
            {
                userDto.Avaible=false;
                await _userRepository.UpdateAsync(userDto);
                throw new Exception("ERROR: No es posible eliminar el usuario debido posee ordenes vinculadas, se ha establecido como usuario no disponible");
            }
            await _userRepository.DeleteAsync(userDto);
            return $"El usuario {userDto.Name} {userDto.LastName} ha sido eliminado con éxito.";
        }
    }
}
