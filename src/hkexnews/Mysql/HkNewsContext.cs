using hkexnews.Model;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace hkexnews.Mysql
{
    /// <summary>
    /// Mysql 
    /// </summary>
    public class HkNewsContext : DbContext
    {

        public DbSet<Records> Records { get; set; }

        public DbSet<DateSaved> DateSaved { get; set; }

        public DbSet<Names> Names { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=207.148.115.130;database=hknews;uid=yefuchao;pwd=eWVmdWNoYW8=");
        }
    }
}
