using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class UserRepositoryTests
    {
        [Fact]
        public void User_GetAll_ReturnsNonZero_And_CorrectFirstValues()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);

            IEnumerable<User> users = repo.GetAllAsync().Result;

            Assert.True(users.Count() != 0);

            User user = repo.GetById(1).Result;

            Assert.Equal("Begoña", user.DisplayName);
            Assert.Equal("begona@test.local", user.Email);
        }

        [Fact]
        public void add_new_userAsync_and_remove()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);

            User user = new User()
            {
                Id=99999999,
                DisplayName="bego",
                Email="begosut@gmail.com",
                Password="1234"
            };
            repo.Add(user);
            repo.SaveChangesAsync().Wait();

            user = repo.GetById(99999999).Result;
            Assert.Equal("bego", user.DisplayName);
            Assert.Equal("begosut@gmail.com", user.Email);
            Assert.Equal("1234", user.Password);

            repo.Remove(user);
            repo.SaveChangesAsync().Wait();

            user = repo.GetById(99999999).Result;
            Assert.Null(user);
        }

        [Fact]
        public void update_user()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);

            User user = new User()
            {
                Id = 99999999,
                DisplayName = "bego",
                Email = "begosut@gmail.com",
                Password = "1234"
            };
            repo.Add(user);
            repo.SaveChangesAsync().Wait();

            user = repo.GetById(99999999).Result;
            user.Password = "1111";
            repo.Update(user);
            repo.SaveChangesAsync().Wait();

            user = repo.GetById(99999999).Result;
            Assert.Equal("1111", user.Password);

            repo.Remove(user);
            repo.SaveChangesAsync().Wait();

        }

        [Fact]
        public void get_users_search()
        {
            DataContext db = new DataContext();
            UserRepository repo = new UserRepository(db);

            SearchRepository repo2 = new SearchRepository(db);

            Search search = new Search()
            {
                Id=10,
                UserId= null,
                SearchText = "Null pointer"
            };
            repo2.Add(search);
            repo2.SaveChangesAsync().Wait();


            User user = new User()
            {
                Id = 999789999,
                DisplayName = "bego",
                Email = "begosut@gmail.com",
                Password = "1234",
                Searches = new[] {search}
            };

           repo.Add(user);
           repo.SaveChangesAsync().Wait();

           user = repo.GetById(999789999).Result;
           search = user.Searches.First();
           Assert.Equal(10, search.Id);
           Assert.Equal("Null pointer", search.SearchText);

           repo.Remove(user);
           repo.SaveChangesAsync().Wait();

           repo2.Remove(search);
           repo2.SaveChangesAsync().Wait();

        }
    }
}
