using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projekt_RESTfulWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Projekt_RESTfulWebAPI.Data
{
    public class OurDbContext : DbContext
    {
        public OurDbContext(DbContextOptions<OurDbContext> options)
            : base(options)
        {
        }

        public DbSet<GeoMessage> GeoMessages { get; set; }
        public DbSet<ApiToken> ApiTokens { get; set; }

        public async Task Seed(UserManager<User> userManager)
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();

            var user = new User
            {
                FirstName = "Us",
                LastName = "Er"
            };
            await userManager.CreateAsync(user);

            var geoMessage = new GeoMessage
            {
                Message = "Testing",
                Longitude = 10.5,
                Latitude = 5.10
            };

            /*var token = new ApiToken
            {
                Key = new Guid(),
                User = user
            };

            await AddAsync(token);*/
            await AddAsync(geoMessage);
            await SaveChangesAsync();
        }

    }
 }

