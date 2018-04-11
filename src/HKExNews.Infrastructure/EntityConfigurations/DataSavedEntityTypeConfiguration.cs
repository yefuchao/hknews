using HKExNews.Domain.AggregatesModel.DateSaveAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HKExNews.Infrastructure.EntityConfigurations
{
    class DataSavedEntityTypeConfiguration : IEntityTypeConfiguration<DateSaved>
    {
        public void Configure(EntityTypeBuilder<DateSaved> builder)
        {
            builder.ToTable("DateSaved", HKExNewsContext.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);
        }
    }
}
