using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace hkexnews.Model
{
    class HKNewsContext : DbContext
    {
        public DbSet<Records> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Records>()
                .Property(b => b.Code)
                .IsRequired();

            modelBuilder.Entity<Records>()
               .Property(b => b.Date)
               .IsRequired();

            modelBuilder.Entity<Records>()
               .Property(b => b.Num)
               .IsRequired();

            modelBuilder.Entity<Records>()
               .Property(b => b.Rate)
               .IsRequired();

            modelBuilder.Entity<Records>()
               .Property(b => b.Stock_Name)
               .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=sql_server_name_or_address;Initial Catalog=hknews;User id =sa;password=your_password;");
        }
    }
}
