using HKExNews.Domain.AggregatesModel.DateSaveAggregate;
using HKExNews.Domain.AggregatesModel.StockAggregate;
using HKExNews.Domain.SeedWork;
using HKExNews.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HKExNews.Infrastructure
{
    public class HKExNewsContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "hknews";

        public DbSet<Records> Records { get; set; }
        public DbSet<DateSaved> DateSaved { get; set; }

        public HKExNewsContext(DbContextOptions<HKExNewsContext> options) : base(options)
        {

        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecordEnityTypeConfiguration());
        }
    }

    public class HKExNewsContexttDesignFactory : IDesignTimeDbContextFactory<HKExNewsContext>
    {
        public HKExNewsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HKExNewsContext>()
                .UseMySql("server=.;database=hknews;");
            //.UseSqlServer("Server=.;Initial Catalog=hknews;Integrated Security=true");

            return new HKExNewsContext(optionsBuilder.Options);
        }
    }
}
