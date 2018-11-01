using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rawdata.Data.Models;

namespace Rawdata.Service.Models
{
    public class UserDto
    {
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }


        public UserDto()
        {
        }

        public UserDto(User user)
        {
            DisplayName = user.DisplayName;
            CreationDate = user.CreationDate;
            Email = user.Email;
            Password = user.Password;
        }
    }
}
