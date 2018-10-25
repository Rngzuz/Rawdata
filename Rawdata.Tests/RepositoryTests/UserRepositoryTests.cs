using System.Collections.Generic;
using System.Linq;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTests
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
        
            User user = users.First();
        
            Assert.Equal(1, user.Id);
            Assert.Equal("Bob", user.DisplayName);
            Assert.Equal("Bob@Bob.com", user.Email);
        }
        //        
        //        public void Add_New_User()
        //        {
        //            DataContext db = new DataContext();
        //            UserRepository repo = new UserRepository(db);
        //            
        //            User user = new User(99999999,"bego","begosut@gmail.com","1234");
        //            repo.Add(user);
        //            repo.SaveChangesAsync();
        //
        //            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
        //            Assert.Equals("bego", user.DisplayName);
        //            Assert.Equals("begosut@gmail.com", user.Email);
        //            Assert.Equals("1234", user.Password);
        //
        //            repo.Remove(user);
        //            repo.SaveChangesAsync;
        //        }
        //
        //        public void Update_User()
        //        {
        //            DataContext db = new DataContext();
        //            UserRepository repo = new UserRepository(db);
        //            
        //            User user = new User(99999999,"bego","begosut@gmail.com","1234");
        //            repo.Add(user);
        //            repo.SaveChangesAsync();
        //
        //            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
        //            user.Password= "1111";
        //            repo.Update(user);
        //            repo.SaveChangesAsync();
        //
        //            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
        //            Assert.Equals("1111", user.Password);
        //
        //            repo.Remove(user);
        //            repo.SaveChangesAsync;
        //        }
        //
        //        public void Remove_User()
        //        {
        //            DataContext db = new DataContext();
        //            UserRepository repo = new UserRepository(db);
        //            
        //            User user = new User(99999999,"bego","begosut@gmail.com","1234");
        //            repo.Add(user);
        //            repo.SaveChangesAsync();
        //
        //            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
        //            repo.Remove(user);
        //            repo.SaveChangesAsync();
        //
        //            user = repo.GetAllAsync().Result.Where(u => u.Id== 99999999);
        //            Assert.Null(user);
        //        }
    }
}