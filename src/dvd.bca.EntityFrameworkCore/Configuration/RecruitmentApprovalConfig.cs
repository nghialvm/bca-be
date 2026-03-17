using dvd.bca.Entity.Recruitment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dvd.bca.Configuration
{
    public class RecruitmentApprovalConfig : IEntityTypeConfiguration<RecruitmentApproval>
    {
        public void Configure(EntityTypeBuilder<RecruitmentApproval> builder)
        {
            builder.HasIndex(x => x.RecruitmentRequestId);
            builder.HasIndex(x => new { x.RecruitmentRequestId, x.StepOrder });

            builder.HasOne(x => x.RecruitmentRequest)
                   .WithMany(x => x.RecruitmentApprovals)
                   .HasForeignKey(x => x.RecruitmentRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
