using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Dashboard.Models
{
    public class UserData
    {
        [Key]
        public long dataId { get; set; }

        public int wins { get; set; }

        public int loses { get; set; }

        public int draws { get; set; }
        public int userRateCounter { get; set; }

        public int ratePoints { get; set; }

        public string year { get; set; }

        public long userId { get; set; }
        public User user { get; set; }


    }
}
