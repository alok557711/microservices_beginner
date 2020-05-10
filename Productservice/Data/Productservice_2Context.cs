using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Productservice_2.Models
{
    public class Productservice_2Context : DbContext
    {
        public Productservice_2Context (DbContextOptions<Productservice_2Context> options)
            : base(options)
        {
        }

        public DbSet<Productservice_2.Models.Product> Product { get; set; }
    }
}
