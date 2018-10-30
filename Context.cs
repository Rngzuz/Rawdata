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
            base.OnConfiguring(builder);
            builder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Post>()
                .ToTable("posts")
                .HasDiscriminator<string>("discriminator")
                .HasValue<Question>("Question")
                .HasValue<Answer>("Answer");

            builder
                .Entity<Question>()
                .HasMany(e => e.Answers)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentId);


            builder
                .Entity<Question>()
                .HasOne(e => e.Link);
        }
    }
}
