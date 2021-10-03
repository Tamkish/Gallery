using Gallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gallery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StoredFile> Files { get; set; }

        //public DbSet<ThumbnailBlob>
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ThumbnailBlob>()
                .HasKey(t => new {t.FileId, t.Type});

            builder.Entity<StoredFile>()
                .HasMany(f => f.Thumbnails).WithOne(t => t.File);

            builder.Entity<Album>()
                .HasKey(a => new {a.OwnerId, a.Name});

            builder.Entity<Album>()
                .HasMany(a => a.Files)
                .WithOne(f => f.Album)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StoredFile>()
                .HasOne(f => f.Album)
                .WithMany(a => a.Files)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasMany(c => c.Children)
                .WithOne(cc => cc.parent)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StoredFile>()
                .HasMany(f => f.Comments)
                .WithOne(c => c.ChildOf)
                .OnDelete(DeleteBehavior.NoAction);


            //builder.Entity<Album>().HasMany(a => a.Files).WithOne(f => f.Album).HasForeignKey(f => new {f.OwnerId, f.Name });
        }
    }
}