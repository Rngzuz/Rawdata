using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTests
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
        //
        //        public void Add_New_Comment()
        //        {
        //            DataContext db = new DataContext();
        //            CommentRepository repo = new CommentRepository(db);
        //            
        //            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
        //            repo.Add(comment);
        //            repo.SaveChangesAsync();
        //
        //            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
        //            Assert.equals(2, comment.Score);
        //            Assert.equals("Null pointer", comment.Text);
        //            Assert.equals(2, comment.AuthorId);
        //            Assert.equals(231855, comment.PostId);
        //
        //            repo.Remove(comment);
        //            repo.SaveChangesAsync;
        //
        //        }
        //
        //        public void Comment_Remove()
        //        {
        //            DataContext db = new DataContext();
        //            CommentRepository repo = new CommentRepository(db);
        //            
        //            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
        //            repo.Add(comment);
        //            repo.SaveChangesAsync();
        //
        //            Comment comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
        //            repo.Remove(comment);
        //            repo.SaveChangesAsync();
        //
        //            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
        //            Assert.Null(comment);
        //
        //        } 
        //
        //        public void Comment_Update()
        //        {
        //            DataContext db = new DataContext();
        //            CommentRepository repo = new CommentRepository(db);
        //            
        //            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
        //            repo.Add(comment);
        //            repo.SaveChangesAsync();
        //
        //            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
        //            comment.Text="Test";
        //            repo.Update(comment);
        //            repo.SaveChangesAsync();
        //
        //            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
        //            assert.Equals("Test", comment.Text);
        //
        //            repo.Remove(comment);
        //            repo.SaveChangesAsync;
        //
        //        }
        //
    }
}