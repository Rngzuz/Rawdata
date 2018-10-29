using System;
using System.Collections.Generic;

namespace Rawdata.Data.Models
{
    public class DeactivatedUser
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DeactivationDate { get; set; }

        public ICollection<DeactivatedFavoriteComment> DeactivatedFavoriteComments { get; set; }
        public ICollection<DeactivatedSearch> DeactivatedSearches { get; set; }
    }
}