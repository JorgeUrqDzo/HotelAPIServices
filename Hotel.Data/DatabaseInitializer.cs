using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!context.Users.Any())
            {
                context.AddRange(GetUsers(context));
                context.SaveChanges();
            }
        }

        private static IEnumerable<User> GetUsers(HotelDbContext context)
        {
            return new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "jorgedzo_17@hotmail.com",
                    Password = "o3FIQbUlkZ6g5sdKoy0ZiXg4icmRk7mTGzV5We02MsAoHS389ZR9jOf1Wn4veAGtot+VTkFsri4=",
                    IsActive = true,
                    IsRemoved = false,
                    RoleId = context.Roles.First(r => r.Identifier == RolesIdentifiers.Admin).Id,
//                    CreatedBy = Guid.Empty,
                    CreatedDate = DateTime.UtcNow,
//                    UpdatedBy = Guid.Empty,
                    UpdatedDate = DateTime.UtcNow
                }
            };
        }

        private static IEnumerable<Role> GetRoles()
        {
            return new List<Role>
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
                }
            };
        }
    }
}