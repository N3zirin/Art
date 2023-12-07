using Art.Entities;
using Microsoft.EntityFrameworkCore;

namespace Art.Data
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Art.Entities.Artist> Artists { get; set; }
        public DbSet<Art.Entities.Picture> Pictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // silmirik

            modelBuilder.Entity<Picture>() // fluent api, eksini de yazmaq olar
                .HasOne<Artist>(art => art.Artist)
                .WithMany(cst => cst.Pictures)
                .HasForeignKey(dp => dp.ArtistId)
                .OnDelete(DeleteBehavior.Cascade); // cedveller arasi elaqe var ve
                                                   // her hansnisa silende diger cedvele mudaxile edilmesin
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        }


    }
}
