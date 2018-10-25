using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTests
{
    public class PostRepositoryTests
    {
        [Fact]
        public void Posts_GetAll_ReturnsNonZero()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
        
            IEnumerable<Post> posts = repo.GetAllAsync().Result;
            Assert.True(posts.Count() != 0);
        
            Post post = posts.First();
            Assert.Equal(19, post.Id);
            Assert.True(post.ChildrenPosts.Count() != 0);
            Assert.Equal(531, post.AcceptedAnswer.Id);
        
            Assert.True(post.PostTags.Count() != 0);
            Assert.Equal("algorithm", post.PostTags.First().TagName);
        
            Assert.True(post.LinkedToPosts.Count() != 0);
            Assert.Equal(1053, post.LinkedToPosts.First().LinkedId);
        
            Assert.True(post.LinkedByPosts.Count() != 0);
            Assert.Equal(841646, post.LinkedByPosts.First().PostId);
        }
        //
        //        public void Add_New_Post()
        //        {
        //            DataContext db = new DataContext();
        //            PostRepository repo = new PostRepository(db);
        //            AuthorRepository repo2 = new AuthorRepository(db);
        //
        //            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
        //            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
        //            repo.Add(post);
        //            repo.SaveChangesAsync();
        //
        //            post= repo.GetAllAsync().Result().Where(p => p.Id==99999999);
        //            Assert.Equals(1, post.TypeId);
        //            Assert.Equals(2, post.Score);
        //            Assert.Equals("I am getting a null pointer at line 9", post.Body);
        //            Assert.Equals("Null pointer", post.Title);
        //            Assert.Null(post.Parent);
        //            Assert.Null(post.ParentId);
        //            Assert.Equals(2, post.AuthorId);
        //
        //            repo.Remove(post);
        //            repo.SaveChangesAsync;
        //        }
        //
        //        public void Update_Post()
        //        {
        //            DataContext db = new DataContext();
        //            PostRepository repo = new PostRepository(db);
        //            AuthorRepository repo2 = new AuthorRepository(db);
        //
        //            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
        //            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
        //            repo.Add(post);
        //            repo.SaveChangesAsync();
        //
        //            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
        //            post.Title="Test";
        //            repo.Update(post);
        //            repo.SaveChangesAsync();
        //
        //            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
        //            Assert.Equals("Test", post.Title);
        //
        //            repo.Remove(post);
        //            repo.SaveChangesAsync;
        //        }
        //
        //        public void Remove_Post()
        //        {
        //            DataContext db = new DataContext();
        //            PostRepository repo = new PostRepository(db);
        //            AuthorRepository repo2 = new AuthorRepository(db);
        //
        //            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
        //            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
        //            repo.Add(post);
        //            repo.SaveChangesAsync();
        //
        //            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
        //
        //            repo.Remove(post);
        //            repo.SaveChangesAsync();
        //
        //            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
        //            Assert.Null(post);
        //
        //        }
    }
}