using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Infrastructure.HashProviders;
using Store.Data.Entities;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            IPasswordHashProvider hashProvider = new BCryptHashProvider();

            User adminUser = new User();
            adminUser.IsActive = true;
            adminUser.Login = "admin";
            adminUser.Password = hashProvider.GenerateHash("admin");
            adminUser.CreatedOn = DateTimeOffset.UtcNow;

            using (var dbContext = new StoreDbContext())
            {
                var adminRole = dbContext.Set<Role>().FirstOrDefault(x => x.ShortName == "Administrator");

                adminUser.Roles.Add(adminRole);

                dbContext.Users.Add(adminUser);
                
                dbContext.SaveChanges();
            }
        }
    }
}
