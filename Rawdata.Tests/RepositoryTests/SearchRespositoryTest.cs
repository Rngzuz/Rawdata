using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories;
using Xunit;

namespace Rawdata.Tests.RepositoryTestsFolder
{
    public class SearchRespositoryTest
    {
        [Fact]
        public void Search_GetAll_ReturnsNonZero_And_CorrectFirstValues()
        {
            DataContext db = new DataContext();
            SearchRepository repo = new SearchRepository(db);

            IEnumerable<Search> searches = repo.GetAllAsync().Result;

            Assert.True(searches.Count() != 0);

            Search search = repo.GetAllAsync().Result.Single(a => a.Id == 1);
            Assert.Equal(1, search.Id);
            Assert.Equal(1, search.UserId);
            Assert.Equal("Null pointer", search.SearchText);
            
        }

        [Fact]
        public void Search_AddNewSearch_And_Remove_Success()
        {
            DataContext db = new DataContext();
            SearchRepository repo = new SearchRepository(db);

            Search search = new Search()
            {
                Id = 2,
                UserId = 2,
                SearchText = "Test"
            };

            repo.Add(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            Assert.Equal(2,search.UserId);
            Assert.Equal("Test", search.SearchText);

            repo.Remove(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            Assert.Null(search);
        }

        [Fact]
        public void Search_Update_Success()
        {
            DataContext db = new DataContext();
            SearchRepository repo = new SearchRepository(db);

            Search search = new Search()
            {
                Id = 2,
                UserId = 2,
                SearchText = "Test"
            };

            repo.Add(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            search.SearchText = "Test2";

            repo.Update(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            Assert.Equal("Test2", search.SearchText);

            repo.Remove(search);
            repo.SaveChangesAsync().Wait();
        }
    }
}
