using Art.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Art.Configurations
{
    public class ArtistConfig: IEntityTypeConfiguration<Artist>//inline modelcreating yazmagin qarsisini alir,
                                                               //entitylere gore ayrilmaga vuset yaradir
    {//burda db'da artiq yer tutmasin deye propertileri restrict edirik
        public void Configure(EntityTypeBuilder<Artist> builder)//core un class'di
        {
            builder.Property(a => a.ArtistName).IsRequired().HasMaxLength(96);
        }
    }
}
