using fileserver.api.Models;
using Microsoft.EntityFrameworkCore;

namespace fileserver.api
{
    public class FilesDbContext : DbContext
    {
        public FilesDbContext(DbContextOptions<FilesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        }

        public DbSet<User> Users { get; set; }
    }
}