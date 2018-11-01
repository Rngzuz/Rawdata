using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Rawdata.Service.Models
{
    public class RegisterDto
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [Required, DataType(DataType.EmailAddress), JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [Required, MinLength(1), MaxLength(255), JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
