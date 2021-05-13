using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Projekt_RESTfulWebAPI.Data
{
    public class OurDbContext : DbContext
    {
        public OurDbContext(DbContextOptions<OurDbContext> options)
            : base(options)
        {
        }
    }
}
