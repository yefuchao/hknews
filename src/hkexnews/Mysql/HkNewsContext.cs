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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=your_address;database=hknews;uid=yefuchao;pwd=your_password");
        }
    }
}
