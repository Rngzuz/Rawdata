using System;
using System.Collections.Generic;
using System.Text;

namespace Rawdata.Data.Models
{
    public class DeactivatedSearch
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public DeactivatedUser DeactivatedUser { get; set; }

        public string SearchText { get; set; }
    }
}
