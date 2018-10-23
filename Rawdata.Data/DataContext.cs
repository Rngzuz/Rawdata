using System;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;

namespace Rawdata.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FavoriteComment> FavoriteComments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Search> Searches { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres");
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        // }
    }
}