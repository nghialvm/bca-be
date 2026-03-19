using dvd.bca.Entity.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dvd.bca.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.HasIndex(x => x.EmployeeCode).IsUnique();

            builder.HasOne(x => x.Candidate)
                .WithMany()
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Application)
                .WithMany()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Offer)
                .WithMany()
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.JobPosition)
                .WithMany()
                .HasForeignKey(x => x.JobPositionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
