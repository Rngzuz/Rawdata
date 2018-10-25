using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests
{
    public class RepositoryTests
    {

        /**
         * AUTHOR TESTS
        **/
        [Fact]
        public void Author_GetAll_ReturnsNonZero_And_CorrectFirstValues()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);

            IEnumerable<Author> authors = repo.GetAllAsync().Result;

            Assert.True(authors.Count() != 0);

            Author author = repo.GetAllAsync().Result.First();
            Assert.Equal(3719, author.Id);
            Assert.Equal("Daemin", author.DisplayName);
            Assert.Equal("Australia", author.Location);
            Assert.Equal(32, author.Age);

            Assert.True(author.Posts.Count != 0);
        }
        
        /**
         * COMMENTS TESTS
         */
        [Fact]
        public void Comments_GetAll_ReturnsNonZero()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);

            IEnumerable<Comment> comments = repo.GetAllAsync().Result;

            Assert.True(comments.Count() != 0);
        }

        /**
         * POST TESTS
         */
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

        /**
        * USER TESTS
        */
        [Fact]
        public void User_GetAll_ReturnsNonZero_And_CorrectFirstValues()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);

            IEnumerable<User> users = repo.GetAllAsync().Result;

            Assert.True(users.Count() != 0);

            User user = users.First();

            Assert.Equal(1, user.Id);
            Assert.Equal("Bob", user.DisplayName);
            Assert.Equal("Bob@Bob.com", user.Email);
        }
       
    }
    
}
