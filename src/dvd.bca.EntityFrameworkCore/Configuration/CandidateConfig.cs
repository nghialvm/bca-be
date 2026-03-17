using dvd.bca.Entity.CandidateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dvd.bca.Configuration
{
    public class CandidateConfig : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasIndex(x => x.CandidateCode).IsUnique();
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.PhoneNumber);

            builder.HasMany(x => x.CandidateDocuments)
                   .WithOne(x => x.Candidate)
                   .HasForeignKey(x => x.CandidateId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Applications)
                   .WithOne(x => x.Candidate)
                   .HasForeignKey(x => x.CandidateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
