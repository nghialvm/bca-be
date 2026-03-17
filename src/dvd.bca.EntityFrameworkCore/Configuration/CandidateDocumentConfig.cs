using dvd.bca.Entity.CandidateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class CandidateDocumentConfig : IEntityTypeConfiguration<CandidateDocument>
    {
        public void Configure(EntityTypeBuilder<CandidateDocument> builder)
        {
            builder.HasIndex(x => x.CandidateId);

            builder.HasOne(x => x.Candidate)
                   .WithMany(x => x.CandidateDocuments)
                   .HasForeignKey(x => x.CandidateId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
