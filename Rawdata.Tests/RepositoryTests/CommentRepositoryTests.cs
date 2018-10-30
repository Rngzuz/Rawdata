using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class CommentRepositoryTests
    {
        [Fact]
        public void Comments_GetAll_ReturnsNonZero()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);
        
            IEnumerable<Comment> comments = repo.GetAllAsync().Result;
        
            Assert.True(comments.Count() != 0);
        }

        [Fact]
        public void Add_New_Comment_And_Remove()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);

            Comment comment = new Comment()
            {
                Id= 999999,
                Score=2,
                Text="Null pointer",
                AuthorId= 312896,
                PostId=19
            };
            repo.Add(comment);
            repo.SaveChangesAsync().Wait();

            comment = repo.GetById(999999).Result;
            Assert.Equal(2, comment.Score);
            Assert.Equal("Null pointer", comment.Text);
            Assert.Equal(312896, comment.AuthorId);
            Assert.Equal(19, comment.PostId);

            repo.Remove(comment);
            repo.SaveChangesAsync().Wait();

            comment = repo.GetById(999999).Result;
            Assert.Null(comment);

        }

        [Fact]

        public void Comment_Update()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);

            Comment comment = new Comment()
            {
                Id = 999999,
                Score = 2,
                Text = "Null pointer",
                AuthorId = 312896,
                PostId = 19
            };
            repo.Add(comment);
            repo.SaveChangesAsync().Wait();

            comment = repo.GetById(999999).Result;
            comment.Text = "Test";
            repo.Update(comment);
            repo.SaveChangesAsync().Wait();

            comment = repo.GetById(999999).Result;
            Assert.Equal("Test", comment.Text);

            repo.Remove(comment);
            repo.SaveChangesAsync().Wait();

        }

    }
}