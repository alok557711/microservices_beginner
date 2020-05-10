using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microservices_1.Models
{
    public class Microservices_1Context : DbContext
    {
        public Microservices_1Context (DbContextOptions<Microservices_1Context> options)
            : base(options)
        {
        }

        public DbSet<Microservices_1.Models.User> User { get; set; }
    }
}
