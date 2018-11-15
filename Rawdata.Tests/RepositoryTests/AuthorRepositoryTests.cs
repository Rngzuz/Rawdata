using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class AuthorRepositoryTests
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

            Author author = repo.GetAllAsync().Result.Single(a => a.Id == 3719);
            Assert.Equal(3719, author.Id);
            Assert.Equal("Daemin", author.DisplayName);
            Assert.Equal("Australia", author.Location);
            Assert.Equal(32, author.Age);

            Assert.True(author.Posts.Count != 0);
        }

        [Fact]
        public void Author_AddNewAuthor_And_Remove_Success()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);
            Author author = new Author()
            {
                Id = 129999999,
                DisplayName = "Bego",
                Location = "Roskilde"
            };

            repo.Add(author);
            repo.SaveChangesAsync().Wait();

            author = repo.GetById(129999999).Result;
            Assert.Equal("Bego", author.DisplayName);
            Assert.Equal("Roskilde", author.Location);

            repo.Remove(author);
            repo.SaveChangesAsync().Wait();

            author = repo.GetById(129999999).Result;
            Assert.Null(author);
        }

        [Fact]
        public void Author_Update_Success()
        {
            DataContext db = new DataContext();
            AuthorRepository repo = new AuthorRepository(db);

            Author author = new Author()
            {
                Id = 129999999,
                DisplayName = "Bego",
                Location = "Roskilde"
            };
            repo.Add(author);
            repo.SaveChangesAsync().Wait();

            author = repo.GetById(129999999).Result;

            Assert.Equal("Bego", author.DisplayName);
            Assert.Null(author.Age);

            author.Age = 20;
            repo.Update(author);
            repo.SaveChangesAsync().Wait();

            author = repo.GetById(129999999).Result;
            Assert.Equal(20, author.Age);

            repo.Remove(author);
            repo.SaveChangesAsync().Wait();
        }
    }
}