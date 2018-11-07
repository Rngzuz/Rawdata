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
        public void Test_GetUserById()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            Task<User> user  = repo.GetUserById(1);

            Assert.Equal("Begoña", user.Result.DisplayName);
            Assert.Equal("begona@test.local", user.Result.Email);
        }

        [Fact]
        public void Test_GetUserByEmail()
        {
            DataContext db = new DataContext();
            UserService repo = new UserService(db);

            Task<User> user = repo.GetUserByEmail("begona@test.local");

            Assert.Equal("Begoña", user.Result.DisplayName);
            Assert.Equal(1, user.Result.Id);
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
    }
}
