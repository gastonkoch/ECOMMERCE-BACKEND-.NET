using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T> where T : class
    {
        public readonly ApplicationDbContext _applicationDbContext;
        public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext; 
        }
    }
}
