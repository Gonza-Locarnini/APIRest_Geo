using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest_Geo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Geo> Geo { get; set; }
    }
}
