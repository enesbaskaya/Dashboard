using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Dashboard_Sonsuz.Models
{
    public class Team
    {
        [Key]
        public long teamId { get; set; }

        public string name { get; set; }

        public string shortName { get; set; }

        public string players { get; set; }

        public string avatarPath { get; set; }

        public long userId { get; set; }
        public User user { get; set; }

    }
}
