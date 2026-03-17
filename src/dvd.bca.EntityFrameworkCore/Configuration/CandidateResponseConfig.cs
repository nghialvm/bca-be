using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class CandidateResponseConfig : IEntityTypeConfiguration<CandidateResponse>
    {
        public void Configure(EntityTypeBuilder<CandidateResponse> builder)
        {
            builder.ToTable("candidateResponses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.ApplicationId).HasColumnName("applicationId").IsRequired();
            builder.Property(x => x.OfferId).HasColumnName("offerId");
            builder.Property(x => x.ResponseType).HasColumnName("responseType").HasMaxLength(50).IsRequired();
            builder.Property(x => x.ResponseTime).HasColumnName("responseTime").IsRequired();

            builder.HasIndex(x => x.ApplicationId);
            builder.HasIndex(x => x.OfferId);

            builder.HasOne(x => x.Application)
                   .WithMany(x => x.CandidateResponses)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Offer)
                   .WithMany(x => x.CandidateResponses)
                   .HasForeignKey(x => x.OfferId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
