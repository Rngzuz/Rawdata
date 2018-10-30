using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rawdata.Service.Models
{
    public class UserDTO
    {
        [JsonProperty(PropertyName = "displayName")]
        public int DisplayName { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public int CreationDate { get; set; }

        [JsonProperty(PropertyName = "email")]
        public int Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public int Password { get; set; }

        }
}
