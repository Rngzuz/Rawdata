using System;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;

namespace Rawdata.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        //        public DbSet<FavoriteComment> FavoriteComments { get; set; }
        //        public DbSet<Search> Searches { get; set; }
        //        public DbSet<Tag> Tags { get; set; }
        //        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres;Password=123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            BuildAuthorConfig(modelBuilder);
            BuildCommentConfig(modelBuilder);
            BuildPostConfig(modelBuilder);
        }

        private void BuildAuthorConfig(ModelBuilder builder)
        {
            builder.Entity<Author>().ToTable("authors");
            builder.Entity<Author>().HasKey(a => a.Id);

            builder.Entity<Author>().Property(a => a.Id).HasColumnName("id");
            builder.Entity<Author>().Property(a => a.DisplayName).HasColumnName("display_name");
            builder.Entity<Author>().Property(a => a.CreationDate).HasColumnName("creation_date");
            builder.Entity<Author>().Property(a => a.Location).HasColumnName("location");
            builder.Entity<Author>().Property(a => a.Age).HasColumnName("age");
        }

        private void BuildCommentConfig(ModelBuilder builder)
        {
            builder.Entity<Comment>().ToTable("comments");
            builder.Entity<Comment>().HasKey(c => c.Id);

            builder.Entity<Comment>().Property(c => c.Id).HasColumnName("id");
            builder.Entity<Comment>().Property(c => c.Score).HasColumnName("score");
            builder.Entity<Comment>().Property(c => c.PostId).HasColumnName("post_id");
            builder.Entity<Comment>().Property(c => c.Text).HasColumnName("text");
            builder.Entity<Comment>().Property(c => c.CreationDate).HasColumnName("creation_date");
            builder.Entity<Comment>().Property(c => c.AuthorId).HasColumnName("author_id");

            builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_author_id_fkey");

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        private void BuildPostConfig(ModelBuilder builder)
        {
            builder.Entity<Post>().ToTable("posts");
            builder.Entity<Post>().HasKey(p => p.Id);

            builder.Entity<Post>().Property(p => p.Id).HasColumnName("id");
            builder.Entity<Post>().Property(p => p.TypeId).HasColumnName("type_id");
            builder.Entity<Post>().Property(p => p.CreationDate).HasColumnName("creation_date");
            builder.Entity<Post>().Property(p => p.ClosedDate).HasColumnName("closed_date");
            builder.Entity<Post>().Property(p => p.Score).HasColumnName("score");
            builder.Entity<Post>().Property(p => p.Body).HasColumnName("body");
            builder.Entity<Post>().Property(p => p.Title).HasColumnName("title");
            builder.Entity<Post>().Property(p => p.ParentId).HasColumnName("parent_id");
            builder.Entity<Post>().Property(p => p.AuthorId).HasColumnName("author_id");
            builder.Entity<Post>().Property(p => p.AcceptedAnswerId).HasColumnName("accepted_answer_id");

            builder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("posts_author_id_fkey");

            builder.Entity<Post>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.ChildrenPosts)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Post>()
                .HasOne(p => p.AcceptedAnswer);
        }
    }
}