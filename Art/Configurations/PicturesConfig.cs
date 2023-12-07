using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Art.Entities;

namespace Art.Configuration
{
    public class PicturesConfig : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(96);
            builder.Property(s => s.ArtistId).IsRequired().HasMaxLength(10);

        }
    }
    
}
