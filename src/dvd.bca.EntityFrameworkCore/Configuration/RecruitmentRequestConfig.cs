using dvd.bca.Entity.Recruitment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dvd.bca.Configuration
{
    public class RecruitmentRequestConfig : IEntityTypeConfiguration<RecruitmentRequest>
    {
        public void Configure(EntityTypeBuilder<RecruitmentRequest> builder)
        {
            builder.HasIndex(x => x.RequestCode).IsUnique();
            builder.HasIndex(x => x.DepartmentId);
            builder.HasIndex(x => x.PositionId);
            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.Department)
                   .WithMany(x => x.RecruitmentRequests)
                   .HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.JobPosition)
                   .WithMany(x => x.RecruitmentRequests)
                   .HasForeignKey(x => x.PositionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RecruitmentApprovals)
                   .WithOne(x => x.RecruitmentRequest)
                   .HasForeignKey(x => x.RecruitmentRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Applications)
                   .WithOne(x => x.RecruitmentRequest)
                   .HasForeignKey(x => x.RecruitmentRequestId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
