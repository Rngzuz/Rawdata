using Xunit;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Rawdata.Service.Profiles;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Rawdata.Data.Models;

namespace Rawdata.Tests
{
    public class SearchControllerTests : Setup
    {
        private readonly SearchController Controller;

        public SearchControllerTests(ITestOutputHelper output) : base(output)
        {
            Controller = new SearchController(DtoMapper, SearchResultService, UserService) {
                Url = UrlHelper,
                ControllerContext = CreateControllerContext()
            };
        }

        [Fact]
        public async Task GetExactMatch_Should_Succeed()
        {
            var paging = new Paging { Words = new[] { "unit", "test" }, Size = 50 };
            var result = await Controller.GetExactMatch(paging) as OkObjectResult;
            var searchResult = JObject.FromObject(result.Value);

            Assert.Equal(1, searchResult["currentPage"].Value<int>());
            Assert.Equal(50, searchResult["items"].Value<JArray>().Count);
        }

        [Fact]
        public async Task GetBestMatch_Should_Succeed()
        {
            var paging = new Paging { Words = new[] { "java", "spring" }, Page = 2, Size = 60 };
            var result = await Controller.GetBestMatch(paging) as OkObjectResult;
            var searchResult = JObject.FromObject(result.Value);

            Assert.Equal(2, searchResult["currentPage"].Value<int>());
            Assert.Equal(60, searchResult["items"].Value<JArray>().Count);
        }

        [Fact]
        public async Task GetRankedWeighted_Should_Succeed()
        {
            var paging = new Paging { Words = new[] { "test" }, Page = 3, Size = 20 };
            var result = await Controller.GetRankedWeightedMatch(paging) as OkObjectResult;
            var searchResult = JObject.FromObject(result.Value);

            Assert.Equal(3, searchResult["currentPage"].Value<int>());
            Assert.Equal(20, searchResult["items"].Value<JArray>().Count);
        }

        [Fact]
        public async Task GetWords_Should_Succeed()
        {
            var result = await Controller.GetWords("test", 20) as OkObjectResult;

            Assert.IsType<List<WeightedKeyword>>(result.Value);
            Assert.Equal(20, (result.Value as List<WeightedKeyword>).Count);
        }
    }
}
