using System;
using System.Collections.Generic;

namespace Rawdata.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<FavoriteComment> FavoriteComments { get; set; }
        public ICollection<FavoritePost> FavoritePosts { get; set; }
        public ICollection<Search> Searches { get; set; }
    }
}