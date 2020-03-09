using CoreMVC.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreMVC.Data.Models
{
    public class CoreMVCDbContext : IdentityDbContext
    {
        public CoreMVCDbContext(DbContextOptions<CoreMVCDbContext> options) : base(options) 
        {   
        }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
