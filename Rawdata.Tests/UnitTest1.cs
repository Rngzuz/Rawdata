using System;
using Xunit;
using Rawdata.Service.Models;

namespace Rawdata.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var paging = new Paging();
            Assert.IsType<Paging>(paging);
        }
    }
}
