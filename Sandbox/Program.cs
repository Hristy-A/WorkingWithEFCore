using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new StoreDbContext())
            {
                // data manipulations here
                
                dbContext.SaveChanges();
            }
        }
    }
}
