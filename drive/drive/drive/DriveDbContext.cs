﻿using drive.Lib.Models;
using Microsoft.EntityFrameworkCore;
using File = drive.Lib.Models.File;

namespace drive
{
    public class DriveDbContext : DbContext
    {
        public DriveDbContext(DbContextOptions<DriveDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}