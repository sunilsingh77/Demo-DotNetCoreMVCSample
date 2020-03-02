using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreMVC.Data.Models
{
    public class CoreMVCDbContext : DbContext
    {
        public CoreMVCDbContext(DbContextOptions<CoreMVCDbContext> options) : base(options) 
        {   
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
