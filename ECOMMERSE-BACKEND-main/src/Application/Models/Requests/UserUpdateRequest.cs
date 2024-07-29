using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class UserUpdateRequest
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public UserType UserType { get; set; }

        public static User ToEntity(UserUpdateRequest dto)
        {
            return new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Password = dto.Password,
                Email = dto.Email,
                UserType = dto.UserType,
            };
        }
    }
}
