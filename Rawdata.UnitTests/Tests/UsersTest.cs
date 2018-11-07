using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Services;
using Xunit;

namespace Rawdata.UnitTests.Tests
{
    public class UsersTest
    {
        [Fact]
        public async void Test_GetUserById()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            User u = new User()
            {
                DisplayName = "Test",
                Email = "Test@gmail.com",
                Password = "abcd"

            };
            await repo.RegisterUser(u);
            await repo.SaveChangesAsync();

            Task<User> user2  = repo.GetUserById(repo.GetUserByEmail("Test@gmail.com").Result.Id);

            Assert.Equal("Test", user2.Result.DisplayName);
            
            repo.DeleteUser(user2.Result);
            await repo.SaveChangesAsync();
        }

        [Fact]
        public async void Test_GetUserByEmail()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            User user = new User()
            {
                DisplayName = "Test",
                Email = "Test@gmail.com",
                Password = "abcd"

            };
            await repo.RegisterUser(user);
            await repo.SaveChangesAsync();


            Task <User> u = repo.GetUserByEmail("Test@gmail.com");

            Assert.Equal("Test", u.Result.DisplayName);
            
            repo.DeleteUser(u.Result);
            await repo.SaveChangesAsync();
        }


        [Fact]
        public async Task Test_RegisterUser()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            User user = new User()
            {
                DisplayName = "Test",
                Email= "Test@gmail.com",
                Password = "abcd"

            };
            await repo.RegisterUser(user);
            await repo.SaveChangesAsync();

            Task<User> u = repo.GetUserByEmail("Test@gmail.com");

            Assert.Equal("Test", u.Result.DisplayName);
            
            repo.DeleteUser(u.Result);
            await repo.SaveChangesAsync();
        }

        [Fact]
        public async Task Test_GetMarkedPosts()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            IQueryable<MarkedPost> posts = repo.GetMarkedPosts(1);
            MarkedPost post = posts.First();

            Assert.Equal(null, post.Note);
            Assert.Equal(19, post.PostId);

        }


        [Fact]
        public async Task Test_GetMarkedComments()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            IQueryable<MarkedComment> comments = repo.GetMarkedComments(1);
            MarkedComment comment = comments.First();

            Assert.Equal("test", comment.Note);
            Assert.Equal(18728068, comment.CommentId);

        }

        [Fact]
        public async Task test_ToggleMarkedPost()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            IQueryable<MarkedPost> posts = repo.ToggleMarkedPost(1, 71, "test");
            if (!posts.Any())
                posts = repo.ToggleMarkedPost(1, 71, "test");
            MarkedPost post = posts.First();

            Assert.Equal(1, post.UserId);
            Assert.Equal(71, post.PostId);

        }

        [Fact]
        public async Task Test_UpdateMarkedPostNote()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            
            IQueryable<MarkedPost> posts = repo.ToggleMarkedPost(1, 71, "test");
            /*if (!posts.Any())
                posts = repo.ToggleMarkedPost(1, 71, "test");*/
            Task<bool> update = repo.UpdateMarkedPostNote(1, 71, "test2");
            
            Assert.True(update.Result);
            
        }
    }

}
