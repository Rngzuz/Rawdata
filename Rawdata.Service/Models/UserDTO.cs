using System;
using System.ComponentModel.DataAnnotations;

namespace Rawdata.Service.Models
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public UserDtoLinks Links { get; set; }

        public class UserDtoLinks
        {
            public string Self { get; set; }
        }
    }

    public class UserSignInDto
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid e-mail address.")]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "Must be atleast 8 characters.")]
        public string Password { get; set; }
    }

    public class UserRegisterDto : UserSignInDto
    {
        [MinLength(1, ErrorMessage = "Must be atleast 8 characters.")]
        public string DisplayName { get; set; }
    }
}
