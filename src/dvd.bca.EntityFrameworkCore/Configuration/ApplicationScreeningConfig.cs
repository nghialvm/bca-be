using dvd.bca.Entity.ApplicationRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Configuration
{
    public class ApplicationScreeningConfig : IEntityTypeConfiguration<ApplicationScreening>
    {
        public void Configure(EntityTypeBuilder<ApplicationScreening> builder)
        {
            builder.HasIndex(x => x.ApplicationId);

            builder.HasOne(x => x.Application)
                   .WithMany(x => x.ApplicationScreenings)
                   .HasForeignKey(x => x.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
