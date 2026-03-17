using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasIndex(x => x.ApplicationCode).IsUnique();
            builder.HasIndex(x => x.RecruitmentRequestId);
            builder.HasIndex(x => x.CandidateId);
            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.RecruitmentRequest)
                   .WithMany(x => x.Applications)
                   .HasForeignKey(x => x.RecruitmentRequestId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Candidate)
                   .WithMany(x => x.Applications)
                   .HasForeignKey(x => x.CandidateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ApplicationScreenings)
                   .WithOne(x => x.Application)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.InterviewSchedules)
                   .WithOne(x => x.Application)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Offer)
                   .WithOne(x => x.Application)
                   .HasForeignKey<Offer>(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.CandidateResponses)
                   .WithOne(x => x.Application)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
