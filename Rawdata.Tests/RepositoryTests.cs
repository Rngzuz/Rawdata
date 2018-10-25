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
        
        public void Author_Add_New_Author()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);
            Author author = new Author (129999999,"Bego","Roskilde",21);

            repo.Add(author);
            repo.SaveChangesAsync;

            author = repo.GetAllAsync().Result.Where(a => a.Id==129999999);
            Assert.Equal("Bego", author.DisplayName);
            Assert.Equal("Roskilde", author.Location);
            Assert.Equal(21, author.Age);
            Assert.True(author.Posts.Count == 0);
            Assert.True(author.Comments.Count != 0);

            repo.Remove(author);
            repo.SaveChangesAsync;
        }

        public void Author_Update()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);

            Author author = new Author (129999999,"Bego","Roskilde",21);
            repo.Add(author);
            repo.SaveChangesAsync;

            author = repo.GetAllAsync().Result.Where(a => a.Id==129999999);
            author.Age=20;
            repo.Update(author);
            repo.SaveChangesAsync;

            author = repo.GetAllAsync().Result.Where(a => a.Id==129999999);
            Assert.Equals(20, author.Age);

            repo.Remove(author);
            repo.SaveChangesAsync;
        }

        public void Author_Remove()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);

            Author author = repo.GetAllAsync().Result.Where(a => a.Id==129999999);
            repo.Remove(author);
            repo.SaveChangesAsync;

            author = repo.GetAllAsync().Result.Where(a => a.Id==129999999);
            Assert.Null(author);

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

        public void Add_New_Comment()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);
            
            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
            repo.Add(comment);
            repo.SaveChangesAsync();

            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
            Assert.equals(2, comment.Score);
            Assert.equals("Null pointer", comment.Text);
            Assert.equals(2, comment.AuthorId);
            Assert.equals(231855, comment.PostId);

            repo.Remove(comment);
            repo.SaveChangesAsync;

        }

        public void Comment_Remove()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);
            
            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
            repo.Add(comment);
            repo.SaveChangesAsync();

            Comment comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
            repo.Remove(comment);
            repo.SaveChangesAsync();

            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
            Assert.Null(comment);

        } 

        public void Comment_Update()
        {
            DataContext db = new DataContext();
            CommentRepository repo = new CommentRepository(db);
            
            Comment comment = new Comment(99999999, 2, "Null pointer", 2, 231855);
            repo.Add(comment);
            repo.SaveChangesAsync();

            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
            comment.Text="Test";
            repo.Update(comment);
            repo.SaveChangesAsync();

            comment = repo.GetAllAsync().Result.Where(c => c.Id == 99999999);
            assert.Equals("Test", comment.Text);

            repo.Remove(comment);
            repo.SaveChangesAsync;

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
        }

        public void Add_New_Post()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
            AuthorRepository repo2 = new AuthorRepository(db);

            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
            repo.Add(post);
            repo.SaveChangesAsync();

            post= repo.GetAllAsync().Result().Where(p => p.Id==99999999);
            Assert.Equals(1, post.TypeId);
            Assert.Equals(2, post.Score);
            Assert.Equals("I am getting a null pointer at line 9", post.Body);
            Assert.Equals("Null pointer", post.Title);
            Assert.Null(post.Parent);
            Assert.Null(post.ParentId);
            Assert.Equals(2, post.AuthorId);

            repo.Remove(post);
            repo.SaveChangesAsync;
        }

        public void Update_Post()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
            AuthorRepository repo2 = new AuthorRepository(db);

            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
            repo.Add(post);
            repo.SaveChangesAsync();

            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
            post.Title="Test";
            repo.Update(post);
            repo.SaveChangesAsync();

            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
            Assert.Equals("Test", post.Title);

            repo.Remove(post);
            repo.SaveChangesAsync;
        }

        public void Remove_Post()
        {
            DataContext db = new DataContext();
            PostRepository repo = new PostRepository(db);
            AuthorRepository repo2 = new AuthorRepository(db);

            Author author = repo2.GetAllAsync.Result().Where(a => a.Id==2);
            Post post = new Post(99999999,1,2, "I am getting a null pointer at line 9", "Null pointer",null,null, 2, author);
            repo.Add(post);
            repo.SaveChangesAsync();

            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);

            repo.Remove(post);
            repo.SaveChangesAsync();

            post = repo.GetAllAsync().Result().Where(p => p.Id==99999999);
            Assert.Null(post);

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

        public void Add_New_User()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);
            
            User user = new User(99999999,"bego","begosut@gmail.com","1234");
            repo.Add(user);
            repo.SaveChangesAsync();

            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
            Assert.Equals("bego", user.DisplayName);
            Assert.Equals("begosut@gmail.com", user.Email);
            Assert.Equals("1234", user.Password);

            repo.Remove(user);
            repo.SaveChangesAsync;
        }

        public void Update_User()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);
            
            User user = new User(99999999,"bego","begosut@gmail.com","1234");
            repo.Add(user);
            repo.SaveChangesAsync();

            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
            user.Password= "1111";
            repo.Update(user);
            repo.SaveChangesAsync();

            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
            Assert.Equals("1111", user.Password);

            repo.Remove(user);
            repo.SaveChangesAsync;
        }

        public void Remove_User()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);
            
            User user = new User(99999999,"bego","begosut@gmail.com","1234");
            repo.Add(user);
            repo.SaveChangesAsync();

            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
            repo.Remove(user);
            repo.SaveChangesAsync();

            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
            Assert.Null(user);
        }
       
    }
    
}
