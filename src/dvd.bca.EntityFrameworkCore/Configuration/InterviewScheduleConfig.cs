using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class InterviewScheduleConfig : IEntityTypeConfiguration<InterviewSchedule>
    {
        public void Configure(EntityTypeBuilder<InterviewSchedule> builder)
        {
            builder.HasIndex(x => x.ApplicationId);
            builder.HasIndex(x => new { x.ApplicationId, x.RoundNumber });

            builder.HasOne(x => x.Application)
                   .WithMany(x => x.InterviewSchedules)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.InterviewEvaluations)
                   .WithOne(x => x.InterviewSchedule)
                   .HasForeignKey(x => x.InterviewScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
