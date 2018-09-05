using System;
using System.Collections.Generic;
using Hotel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hotel.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(HotelDbContext context)
        {
            //make sure the db is created
            //if it isn't, run the migrations
            context.Database.Migrate();
            
            if (!context.Roles.Any())
            {
                context.AddRange(GetRoles());

                context.SaveChanges();
            }
        }

        private static IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Description = "Administrator",
                    Identifier = RolesIdentifiers.Admin,
                    Order = 1
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Worker",
                    Description = "Worker",
                    Identifier = RolesIdentifiers.Worker,
                    Order = 2
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "WorkerRead",
                    Description = "Worker Read",
                    Identifier = RolesIdentifiers.WorkerRead,
                    Order = 3
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    Description = "Simple User",
                    Identifier = RolesIdentifiers.User,
                    Order = 3
                },
            };

        }
    }
}