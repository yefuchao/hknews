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
            optionsBuilder.UseMySql("server=127.0.0.1;database=hknews;uid=root;pwd=your_password");
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
