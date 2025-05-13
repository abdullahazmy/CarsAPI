using EdufyAPI.Models.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarsAPI.Models
{
    // You can Change DbContext to your desired name
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region DbSets
        // Add your DbSets here
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<FavoriteCar> FavoriteCars { get; set; }
        public virtual DbSet<ImageCar> ImageCars { get; set; }
        #endregion

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Roles Seed
            //modelBuilder.Entity<IdentityRole>(entity =>
            //{
            //    // Add Admin Role
            //    entity.HasData(new IdentityRole
            //    {
            //        Name = "Admin",
            //        NormalizedName = "ADMIN"
            //    });

            //    // Add User Role
            //    entity.HasData(new IdentityRole
            //    {
            //        Name = "User",
            //        NormalizedName = "USER"
            //    });

            //    // Add SuperAdmin Role
            //    entity.HasData(new IdentityRole
            //    {
            //        Name = "SuperAdmin",
            //        NormalizedName = "SUPERADMIN"
            //    });
            //});
            #endregion

            modelBuilder.Entity<Car>()
                .Property(c => c.Year)
                .HasColumnType("date");

            // Add your model configurations here
        }
    }
}
