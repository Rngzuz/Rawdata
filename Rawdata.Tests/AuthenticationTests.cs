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
    public class AuthenticationTests
    {
       
        [Fact]
        public void Authentication_Passing_Validate()
        {
            var value = "HelL0W0rLd";
            var salt = Authentication.CreateSalt();

            var hash = Authentication.CreateHash(value, salt);

            var isValueValid = Authentication.Validate(value, salt, hash);

            Assert.True(isValueValid);
        }

        [Fact]
        public void Authentication_Failing_Validate()
        {
            var value1 = "HelL0W0rLd";
            var value2 = "My#passW0rd";
            var salt = Authentication.CreateSalt();

            var hash = Authentication.CreateHash(value1, salt);

            var isValueValid = Authentication.Validate(value2, salt, hash);

            Assert.False(isValueValid);
        }
    }
}
