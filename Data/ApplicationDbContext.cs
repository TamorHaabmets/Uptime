using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Leg> Legs { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<RouteInfo> RouteInfos { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Company> Companies { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base (options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leg>().HasKey(c => c.Id);
            modelBuilder.Entity<PriceList>().HasKey(c => c.Id);
            modelBuilder.Entity<Reservation>().HasKey(c => c.Id);
            modelBuilder.Entity<Provider>().HasKey(c => c.Id);
            modelBuilder.Entity<RouteInfo>().HasKey(c => c.Id);
            modelBuilder.Entity<Leg>()
                .HasOne(a => a.RouteInfo).WithOne(b => b.Leg)
                .HasForeignKey<RouteInfo>(e => e.Id);
            modelBuilder
                .Entity<Reservation>()
                .HasMany(p => p.Companies)
                .WithMany(p => p.Reservations)
                .UsingEntity(j => j.ToTable("ReservationCompanies"));

            modelBuilder.Entity<Company>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
