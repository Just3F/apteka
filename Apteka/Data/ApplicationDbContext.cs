using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Apteka.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        AppRole,
        int,
        AppUserClaim,
        AppUserRole,
        AppUserLogin,
        AppRoleClaim,
        AppUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProduct>()
                .HasKey(bc => new { bc.UserId, bc.ProductId });

            modelBuilder.Entity<UserProduct>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserProducts)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(bc => bc.UsersProducts)
                .HasForeignKey(bc => bc.ProductId);

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<AppRole>().ToTable("Role");
            modelBuilder.Entity<AppUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<AppUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");
            modelBuilder.Entity<AppUserToken>().ToTable("UserToken");
            modelBuilder.Entity<AppRoleClaim>().ToTable("RoleClaim");

        }
    }

    public class ApplicationUser : IdentityUser<int>
    {
        public ICollection<UserProduct> UserProducts { get; set; }
    }

    public class AppUserClaim : IdentityUserClaim<int> { }

    public class AppUserToken : IdentityUserToken<int> { }

    public class AppUserLogin : IdentityUserLogin<int> { }

    public class AppUserRole : IdentityUserRole<int> { }

    public class AppRole : IdentityRole<int> { }

    public class AppRoleClaim : IdentityRoleClaim<int> { }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
