using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ExternalService.Data.Entities;

namespace ExternalService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Category> tblCategory { get; set; }
        public DbSet<Product> tblProduct { get; set; }
        public DbSet<User> tblUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.StrName).IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.StrUserName).IsUnique();
        }
    }
}
