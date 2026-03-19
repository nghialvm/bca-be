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
            builder.HasOne(x => x.Application)
                   .WithMany(x => x.CandidateResponses)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Offer)
                   .WithMany()
                   .HasForeignKey(x => x.OfferId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.ApplicationId);
            builder.HasIndex(x => x.OfferId);
            builder.HasIndex(x => x.ResponseType);
            builder.HasIndex(x => x.ResponseTime);
        }
    }
}
