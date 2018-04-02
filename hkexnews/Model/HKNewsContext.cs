using hkexnews.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    class HKNewsContext : DbContext
    {
        public DbSet<Records> Records { get; set; }

        public DbSet<DateSaved> DateSaved { get; set; }

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
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-MI4FF793;Initial Catalog=hknews;User ID=sa;password=123456;");
        }
    }
}
