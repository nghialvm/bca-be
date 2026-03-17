using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.ToTable("offers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.ApplicationId).HasColumnName("applicationId").IsRequired();
            builder.Property(x => x.Salary).HasColumnName("salary").HasColumnType("decimal(18,2)");
            builder.Property(x => x.StartDate).HasColumnName("startDate");
            builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(50).IsRequired();

            builder.HasIndex(x => x.ApplicationId).IsUnique();

            builder.HasOne(x => x.Application)
                   .WithOne(x => x.Offer)
                   .HasForeignKey<Offer>(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.CandidateResponses)
                   .WithOne(x => x.Offer)
                   .HasForeignKey(x => x.OfferId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
