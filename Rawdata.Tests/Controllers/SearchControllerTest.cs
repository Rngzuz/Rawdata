using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rawdata.Data;
using Rawdata.Data.Services;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Controllers;
using Rawdata.Service.Profiles;
using Xunit;
using Xunit.Abstractions;

namespace Rawdata.Tests.Controllers
{
    public class SearchControllerTest
    {
        private const string SearchApi = "http://localhost:5000/api/search";

        [Fact]
        public void SearchApi_ExactMatch_Ok()
        {
            var (data, statusCode) = GetArray(SearchApi + "/exact?words=unit&words=test&page=1&size=20");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(20, data.Count);

            Assert.Equal(560, data.First()["score"]);
            Assert.Equal("tchrist", data.First()["authorDisplayName"]);
            Assert.Equal(false, data.First()["marked"]);
        }

        [Fact]
        public void SearchApi_ExactMatch_Ok_GoToParent()
        {
            var (data, statusCode) = GetArray(SearchApi + "/exact?words=unit&words=test&page=1&size=20");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(20, data.Count);

            Assert.Equal(560, data.First()["score"]);
            Assert.Equal("tchrist", data.First()["authorDisplayName"]);
            Assert.Equal(false, data.First()["marked"]);

            Assert.Equal("http://localhost:5000/api/answers/4234491", data.First()["links"]["self"]);
            Assert.Equal("http://localhost:5000/api/questions/4231382", data.First()["links"]["parent"]);

            var (parentData, parentStatusCode) = GetObject(((string) data.First()["links"]["parent"]));

            Assert.Equal(HttpStatusCode.OK, parentStatusCode);
            Assert.Equal(97, parentData["score"]);
            Assert.Equal("Regular expression pattern not matching anywhere in string", parentData["title"]);
        }

        [Fact]
        public void SearchApi_BestMatch_Ok()
        {
            var (data, statusCode) = GetArray(SearchApi + "/best?words=unit&words=test");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(50, data.Count);

            Assert.Equal(560, data.First()["score"]);
            Assert.Equal("tchrist", data.First()["authorDisplayName"]);
            Assert.Equal(false, data.First()["marked"]);
            Assert.Equal(2, data.First()["rank"]);
        }

        [Fact]
        public void SearchApi_RankedMatch_Ok()
        {
            var (data, statusCode) = GetArray(SearchApi + "/ranked?words=unit&words=test");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(50, data.Count);

            Assert.Equal(80, data.First()["score"]);
            Assert.Equal("TcKs", data.First()["authorDisplayName"]);
            Assert.Equal(false, data.First()["marked"]);
            Assert.Equal(0.00029818465540940438, data.First()["rank"]);
        }

        [Fact]
        public void SearchApi_WordToWord_Ok()
        {
            var (data, statusCode) = GetArray(SearchApi + "/words?word=mac");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(100, data.Count);

            Assert.Equal("error", data.First()["word"]);
            Assert.Equal("41", data.First()["weight"]);
        }

        [Fact]
        public void SearchApi_WordContext_Ok()
        {
            var (data, statusCode) = GetArray(SearchApi + "/context?word=mac");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(100, data.Count);

            Assert.Equal("restrict", data.First()["word2"]);
            Assert.Equal("40", data.First()["grade"]);
        }
        
        
        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

    }
}