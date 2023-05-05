using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVCAngularShortener.Models;
using MVCAngularShortener.Models.ViewModels;

namespace MVCAngularShortener.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            string ROLE_ID = "341743f0 - asd2–42de - afbf - 59kmkkmk72cf6";

            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });
        }

        public DbSet<Url> Urls { get; set; }


    }

    public class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullUrl)
                .IsRequired().HasMaxLength(256);
            builder.Property(u => u.ShortUrl)
                .IsRequired();
            builder.Property(u => u.CreatedBy)
                .IsRequired();
            builder.Property(u => u.CreatedDate)
                .IsRequired();
        }
    }
}