using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class InterviewEvaluationConfig : IEntityTypeConfiguration<InterviewEvaluation>
    {
        public void Configure(EntityTypeBuilder<InterviewEvaluation> builder)
        {
            builder.HasIndex(x => x.InterviewScheduleId);
            builder.HasIndex(x => x.ApplicationId);

            builder.HasOne(x => x.InterviewSchedule)
                   .WithMany(x => x.InterviewEvaluations)
                   .HasForeignKey(x => x.InterviewScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Application)
                   .WithMany(x => x.InterviewEvaluations)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
