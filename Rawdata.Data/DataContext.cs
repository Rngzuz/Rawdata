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
        public DbSet<MarkedComment> MarkedComments { get; set; }
        public DbSet<MarkedPost> MarkedPosts { get; set; }
        public DbSet<Search> Searches { get; set; }

        public DbQuery<SearchResult> SearchResults { get; set; }
        public DbQuery<WeightedKeyword> WeightedKeywords { get; set; }
        public DbQuery<WordAssociation> WordAssociations { get; set; }
        public DbQuery<ForceGraphInput> ForceGraphInputs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=stackoverflow;Username=postgres;Password=123");
            // optionsBuilder.UseNpgsql("Server=rawdata.ruc.dk;Port=5432;Database=raw3;Username=raw3;Password=ABAZEKAg");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildSearchResult(modelBuilder);
            BuildWeightedKeywordConfig(modelBuilder);
            BuilWordAssociationConfig(modelBuilder);
            BuildForceGraphInputConfig(modelBuilder);

            //Stackover flow DB
            BuildAuthorConfig(modelBuilder);
            BuildCommentConfig(modelBuilder);
            BuildPostConfig(modelBuilder);
            BuildTagConfig(modelBuilder);
            BuildPostTagConfig(modelBuilder);
            BuildPostLinkConfig(modelBuilder);

            //Application DB
            BuildUserConfig(modelBuilder);
            BuildMarkedCommentConfig(modelBuilder);
            BuildMarkedPostConfig(modelBuilder);
            BuildSearchConfig(modelBuilder);
        }

        private void BuildSearchResult(ModelBuilder builder)
        {
            builder.Query<SearchResult>().Property(m => m.PostId).HasColumnName("post_id");
            builder.Query<SearchResult>().Property(m => m.Rank).HasColumnName("rank");
            // builder.Query<SearchResult>().Property(m => m.Excerpts).HasColumnName("sentences");
            builder.Query<SearchResult>().HasOne(m => m.Post);
        }

        private void BuildWeightedKeywordConfig(ModelBuilder builder)
        {
            builder.Query<WeightedKeyword>().Property(m => m.Word).HasColumnName("word");
            builder.Query<WeightedKeyword>().Property(m => m.Weight).HasColumnName("freq");
        }

        private void BuilWordAssociationConfig(ModelBuilder builder)
        {
            builder.Query<WordAssociation>().Property(m => m.Word1).HasColumnName("word1");
            builder.Query<WordAssociation>().Property(m => m.Word2).HasColumnName("word2");
            builder.Query<WordAssociation>().Property(m => m.Grade).HasColumnName("grade");
        }

        private void BuildForceGraphInputConfig(ModelBuilder builder)
        {
            builder.Query<ForceGraphInput>().Property(m => m.Input).HasColumnName("input");
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

            builder.Entity<Post>()
                .HasDiscriminator<int>("TypeId")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

            builder.Entity<Post>().HasKey(p => p.Id);
            builder.Entity<Post>().Property(p => p.Id).HasColumnName("id");
            builder.Entity<Post>().Property(p => p.TypeId).HasColumnName("type_id");
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
            builder.Entity<PostTag>().ToTable("posts_tags");
            builder.Entity<PostTag>().HasKey(pt => new { pt.TagName, pt.QuestionId });

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
            builder.Entity<PostLink>().HasKey(pl => new { pl.PostId, pl.LinkedId });
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

        private void BuildMarkedCommentConfig(ModelBuilder builder)
        {
            builder.Entity<MarkedComment>().ToTable("marked_comments");
            builder.Entity<MarkedComment>().Property(c => c.UserId).HasColumnName("user_id");
            builder.Entity<MarkedComment>().Property(c => c.CommentId).HasColumnName("comment_id");
            builder.Entity<MarkedComment>().Property(c => c.Note).HasColumnName("note");

            builder.Entity<MarkedComment>().HasKey(c => new { c.UserId, c.CommentId });

            builder.Entity<MarkedComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.MarkedComments)
                .HasForeignKey(c => c.UserId);

            builder.Entity<MarkedComment>()
                .HasOne(c => c.Comment)
                .WithMany(u => u.MarkedComments)
                .HasForeignKey(c => c.CommentId);
        }

        private void BuildMarkedPostConfig(ModelBuilder builder)
        {
            builder.Entity<MarkedPost>().ToTable("marked_posts");
            builder.Entity<MarkedPost>().Property(c => c.UserId).HasColumnName("user_id");
            builder.Entity<MarkedPost>().Property(c => c.PostId).HasColumnName("post_id");
            builder.Entity<MarkedPost>().Property(c => c.Note).HasColumnName("note");

            builder.Entity<MarkedPost>().HasKey(c => new { c.UserId, c.PostId });

            builder.Entity<MarkedPost>()
                .HasOne(c => c.User)
                .WithMany(u => u.MarkedPosts)
                .HasForeignKey(c => c.PostId);

            builder.Entity<MarkedPost>()
                .HasOne(mp => mp.Post)
                .WithMany(p => p.MarkedPosts)
                .HasForeignKey(mp => mp.PostId);
        }

        private void BuildSearchConfig(ModelBuilder builder)
        {
            builder.Entity<Search>().ToTable("searches");

            builder.Entity<Search>().Property(u => u.UserId).HasColumnName("user_id");
            builder.Entity<Search>().Property(u => u.SearchText).HasColumnName("search_text");
            builder.Entity<Search>().HasKey(c => new { c.UserId, c.SearchText });

            builder.Entity<Search>()
                .HasOne(s => s.User)
                .WithMany(s => s.Searches)
                .HasForeignKey(s => s.UserId);
        }
    }
}
