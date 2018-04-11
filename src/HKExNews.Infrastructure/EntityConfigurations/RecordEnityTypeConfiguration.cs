using HKExNews.Domain.AggregatesModel.StockAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Infrastructure.EntityConfigurations
{
    public class RecordEnityTypeConfiguration : IEntityTypeConfiguration<Records>
    {
        public void Configure(EntityTypeBuilder<Records> builder)
        {
            builder.ToTable("records", HKExNewsContext.DEFAULT_SCHEMA);

            builder.HasKey(b => b.Id);

        }
    }
}
