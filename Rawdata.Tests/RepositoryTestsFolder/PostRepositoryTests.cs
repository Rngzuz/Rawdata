using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
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

            Post post = repo.GetById(19).Result;
            Assert.True(post.ChildrenPosts.Count() != 0);
            Assert.Equal(531, post.AcceptedAnswer.Id);
        
            Assert.True(post.PostTags.Count() != 0);
           // The first tag it gets is "Pi"
           // Assert.Equal("algorithm", post.PostTags.First().TagName);
        
            Assert.True(post.LinkedToPosts.Count() != 0);
            Assert.Equal(1053, post.LinkedToPosts.First().LinkedId);
        
            Assert.True(post.LinkedByPosts.Count() != 0);
            Assert.Equal(841646, post.LinkedByPosts.First().PostId);
        }


        [Fact]
        public void Add_New_Post_And_Remove_Post()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
            AuthorRepository repo2 = new AuthorRepository(db);

            Author author = repo2.GetById(2).Result;

            Post post = new Post()
            {
                Id=99999999,
                TypeId=1,
                Score= 2,
                Body= "I am getting a null pointer at line 9",
                Title= "Null pointer",
                ParentId=null,
                Parent= null,
                AuthorId = 2,
                Author= author
            };
            repo.Add(post);
            repo.SaveChangesAsync().Wait();

            post = repo.GetById(99999999).Result;

            Assert.Equal(1, post.TypeId);
            Assert.Equal(2, post.Score);
            Assert.Equal("I am getting a null pointer at line 9", post.Body);
            Assert.Equal("Null pointer", post.Title);
            Assert.Null(post.Parent);
            Assert.Null(post.ParentId);
            Assert.Equal(2, post.AuthorId);

            repo.Remove(post);
            repo.SaveChangesAsync().Wait();
        }

        [Fact]
        public void Update_Post()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
            AuthorRepository repo2 = new AuthorRepository(db);

            Author author = repo2.GetById(2).Result;
            Post post = new Post()
            {
                Id = 99999999,
                TypeId = 1,
                Score = 2,
                Body = "I am getting a null pointer at line 9",
                Title = "Null pointer",
                ParentId = null,
                Parent = null,
                AuthorId = 2,
                Author = author
            };
            repo.Add(post);
            repo.SaveChangesAsync().Wait();

            post = repo.GetById(99999999).Result;
            post.Title = "Test";
            repo.Update(post);
            repo.SaveChangesAsync().Wait();

            post = repo.GetAllAsync().Result.Single(p => p.Id == 99999999);
            Assert.Equal("Test", post.Title);

            repo.Remove(post);
            repo.SaveChangesAsync().Wait();
        }
        
    }
}