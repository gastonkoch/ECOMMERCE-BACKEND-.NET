using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context) : base(context) { }

        public User? GetByUserEmail(string userEmail)
        {
            return _applicationDbContext.Users.SingleOrDefault(p => p.Email == userEmail);
        }

        public virtual async Task<List<User>> GetByName(string name)
        {
            string nameLower = name.ToLower();
            return await _applicationDbContext.Users.Where(p => p.Name.ToLower().Contains(nameLower)).ToListAsync();
        }

        public virtual async Task<List<User>> GetListAvaible()
        {
            return await _applicationDbContext.Users.Where(p => p.Avaible == true).ToListAsync();
        }
    }
}
