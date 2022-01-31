using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Persistence.Configurations
{
    public class WeatherHistoryEntityConfiguration : IEntityTypeConfiguration<WeatherHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<WeatherHistoryEntity> builder)
        {
            builder.ToTable("WeatherHistories");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Request)
                .IsRequired(false);

            builder.Property(c => c.Data)
                .IsRequired(false);

            builder.Property(c => c.ErrorDescription)
                .IsRequired(false);

            builder.Property(c => c.LastUpdateDate)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}
