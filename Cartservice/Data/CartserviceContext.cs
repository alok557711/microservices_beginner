using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cartservice.Models
{
    public class CartserviceContext : DbContext
    {
        public CartserviceContext (DbContextOptions<CartserviceContext> options)
            : base(options)
        {
        }

        public DbSet<Cartservice.Models.Cart> Cart { get; set; }
    }
}
