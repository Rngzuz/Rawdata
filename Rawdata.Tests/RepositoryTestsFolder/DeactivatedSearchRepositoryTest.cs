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
    public class DeactivatedSearchRepositoryTest
    {
        [Fact]
        public void DeactivatedSearch_GetAll_ReturnsNonZero_And_CorrectFirstValues()
        {
            DataContext db = new DataContext();
            DeactivatedSearchRespository repo = new DeactivatedSearchRespository(db);

            IEnumerable<DeactivatedSearch> searches = repo.GetAllAsync().Result;

            Assert.True(searches.Count() != 0);

            DeactivatedSearch search = repo.GetAllAsync().Result.Single(a => a.Id == 10);
            Assert.Equal(10, search.Id);
            Assert.Equal(1, search.UserId);
            Assert.Equal("Null pointer", search.SearchText);

        }

        [Fact]
        public void DeactivatedSearch_AddNewSearch_And_Remove_Success()
        {
            DataContext db = new DataContext();
            DeactivatedSearchRespository repo = new DeactivatedSearchRespository(db);

            DeactivatedSearch search = new DeactivatedSearch()
            {
                Id = 2,
                UserId = 1,
                SearchText = "Test"
            };

            repo.Add(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            Assert.Equal(1, search.UserId);
            Assert.Equal("Test", search.SearchText);

            repo.Remove(search);
            repo.SaveChangesAsync().Wait();

            search = repo.GetById(2).Result;
            Assert.Null(search);
        }

        [Fact]
        public void DeactivatedSearch_Update_Success()
        {
            DataContext db = new DataContext();
            DeactivatedSearchRespository repo = new DeactivatedSearchRespository(db);

            DeactivatedSearch search = new DeactivatedSearch()
            {
                Id = 2,
                UserId = 1,
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
