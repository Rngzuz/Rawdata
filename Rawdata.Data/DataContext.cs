using System;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Models.Relationships;

namespace Rawdata.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<FavoriteComment> FavoriteComments { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<Search> Searches { get; set; }
<<<<<<< HEAD
        
        
=======
>>>>>>> a0ab269601d6658c990ad4ecb7cb189dfcc452bc

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres;Password=123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            //Stackover flow DB
            BuildAuthorConfig(modelBuilder);
            BuildCommentConfig(modelBuilder);
            BuildPostConfig(modelBuilder);
            BuildTagConfig(modelBuilder);
            BuildPostTagConfig(modelBuilder);
            BuildPostLinkConfig(modelBuilder);

            //Application DB

            BuildUserConfig(modelBuilder);
            BuildFavoriteCommentConfig(modelBuilder);
<<<<<<< HEAD
            BuildFavoritePostConfig(modelBuilder);

            BuildSearchConfig(modelBuilder);
            

=======
            BuildSearchConfig(modelBuilder);
            
>>>>>>> a0ab269601d6658c990ad4ecb7cb189dfcc452bc
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
            builder.Entity<Post>()
                .ToTable("posts")
                .HasDiscriminator<int>("type_id")
                .HasValue<Question>(1)
                .HasValue<Answer>(2); 

            builder.Entity<Post>().HasKey(p => p.Id);
            builder.Entity<Post>().Property(p => p.Id).HasColumnName("id");
            builder.Entity<Post>().Property(p => p.CreationDate).HasColumnName("creation_date");
            builder.Entity<Post>().Property(p => p.Score).HasColumnName("score");
            builder.Entity<Post>().Property(p => p.Body).HasColumnName("body");
            builder.Entity<Post>().Property(p => p.AuthorId).HasColumnName("author_id");

            builder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("posts_author_id_fkey");

            //Question post config

            builder.Entity<Question>().Property(q => q.Title).HasColumnName("title");
            builder.Entity<Question>().Property(q => q.ClosedDate).HasColumnName("closed_date");
            builder.Entity<Question>().Property(q => q.AcceptedAnswerId).HasColumnName("accepted_answer_id");
            
            //Answer post config
            builder.Entity<Answer>().Property(a => a.ParentId).HasColumnName("parent_id");

            builder.Entity<Answer>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Answers)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
        }

        private void BuildTagConfig(ModelBuilder builder)
        {
            builder.Entity<Tag>().ToTable("tags");
            builder.Entity<Tag>().HasKey(t => t.Name);
        }

        private void BuildPostTagConfig(ModelBuilder builder)
        {
            builder.Entity<PostTag>().ToTable("post_tags");
            builder.Entity<PostTag>().HasKey(pt => new { pt.TagName, pt.QuestionId});

            builder.Entity<PostTag>().Property(pt => pt.QuestionId).HasColumnName("post_id");
            builder.Entity<PostTag>().Property(pt => pt.TagName).HasColumnName("name");

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Question)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.QuestionId);

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagName);
        }

        private void BuildPostLinkConfig(ModelBuilder builder)
        {
            builder.Entity<PostLink>().ToTable("post_links");
            builder.Entity<PostLink>().HasKey(pl => new { pl.PostId, pl.LinkedId});
            builder.Entity<PostLink>().Property(pl => pl.PostId).HasColumnName("post_id");
            builder.Entity<PostLink>().Property(pl => pl.LinkedId).HasColumnName("link_id");

            builder.Entity<PostLink>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.LinkedToPosts)
                .HasForeignKey(pl => pl.PostId);

            builder.Entity<PostLink>()
                .HasOne(pl => pl.LinkedPost)
                .WithMany(p => p.LinkedByPosts)
                .HasForeignKey(pl => pl.LinkedId);
        }

        private void BuildUserConfig(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<User>().HasKey(u => u.Id);

            builder.Entity<User>().Property(u => u.Id).HasColumnName("id");
            builder.Entity<User>().Property(u => u.DisplayName).HasColumnName("display_name");
            builder.Entity<User>().Property(u => u.CreationDate).HasColumnName("creation_date");
            builder.Entity<User>().Property(u => u.Email).HasColumnName("email");
            builder.Entity<User>().Property(u => u.Password).HasColumnName("password");
        }

        private void BuildFavoriteCommentConfig(ModelBuilder builder)
        {
            builder.Entity<FavoriteComment>().ToTable("favorite_comments");
            builder.Entity<FavoriteComment>().HasKey(c => new {c.UserId, c.CommentId});

            builder.Entity<FavoriteComment>().Property(c => c.UserId).HasColumnName("user_id");
            builder.Entity<FavoriteComment>().Property(c => c.CommentId).HasColumnName("comment_id");

            builder.Entity<FavoriteComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.FavoriteComments)
                .HasForeignKey(c => c.CommentId);
        }

        private void BuildFavoritePostConfig(ModelBuilder builder)
        {
            builder.Entity<FavoritePost>().ToTable("favorite_posts");
            builder.Entity<FavoritePost>().HasKey(c => new { c.UserId, c.PostId });

            builder.Entity<FavoritePost>().Property(c => c.UserId).HasColumnName("user_id");
            builder.Entity<FavoritePost>().Property(c => c.PostId).HasColumnName("post_id");

            builder.Entity<FavoritePost>()
                .HasOne(c => c.User)
                .WithMany(u => u.FavoritePosts)
                .HasForeignKey(c => c.PostId);
        }
        
        private void BuildSearchConfig(ModelBuilder builder)
        {
            builder.Entity<Search>().ToTable("searches");
            builder.Entity<Search>().HasKey(s => s.Id);

            builder.Entity<Search>().Property(u => u.Id).HasColumnName("id");
            builder.Entity<Search>().Property(u => u.UserId).HasColumnName("user_id");
            builder.Entity<Search>().Property(u => u.SearchText).HasColumnName("search_text");

            builder.Entity<Search>()
                .HasOne(s => s.User)
                .WithMany(s => s.Searches)
                .HasForeignKey(s => s.UserId);
        }
<<<<<<< HEAD

        private void BuildDeactivatedSearchConfig(ModelBuilder builder)
        {
            builder.Entity<DeactivatedSearch>().ToTable("deactivated_searches");
            builder.Entity<DeactivatedSearch>().HasKey(s => s.Id);

            builder.Entity<DeactivatedSearch>().Property(u => u.Id).HasColumnName("id");
            builder.Entity<DeactivatedSearch>().Property(u => u.UserId).HasColumnName("user_id");
            builder.Entity<DeactivatedSearch>().Property(u => u.SearchText).HasColumnName("search_text");

            builder.Entity<DeactivatedSearch>()
                .HasOne(s => s.DeactivatedUser)
                .WithMany(s => s.DeactivatedSearches)
                .HasForeignKey(s => s.UserId);
        }

        private void BuildDeactivatedFavoriteCommentConfig(ModelBuilder builder)
        {
            builder.Entity<DeactivatedFavoriteComment>().ToTable("deactivated_favorite_comments");
                builder.Entity<DeactivatedFavoriteComment>().HasKey(c => new { c.DeactivatedUser, c.CommentId });

            builder.Entity<DeactivatedFavoriteComment>().Property(c => c.DeactivatedUser).HasColumnName("user_id");
            builder.Entity<DeactivatedFavoriteComment>().Property(c => c.CommentId).HasColumnName("comment_id");

            builder.Entity<DeactivatedFavoriteComment>()
                .HasOne(c => c.DeactivatedUser)
                .WithMany(u => u.DeactivatedFavoriteComments)
                .HasForeignKey(c => c.CommentId);
        }


=======
        
>>>>>>> a0ab269601d6658c990ad4ecb7cb189dfcc452bc
    }
}