using System;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class Context : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Post>();
        }
    }
}
