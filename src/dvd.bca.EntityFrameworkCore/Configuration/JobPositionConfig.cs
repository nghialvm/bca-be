using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dvd.bca.Configuration
{
    public class JobPositionConfig : IEntityTypeConfiguration<JobPosition>
    {
        public void Configure(EntityTypeBuilder<JobPosition> builder)
        {
            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.DepartmentId);

            builder.HasOne(x => x.Department)
                   .WithMany(x => x.JobPositions)
                   .HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RecruitmentRequests)
                   .WithOne(x => x.JobPosition)
                   .HasForeignKey(x => x.PositionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
