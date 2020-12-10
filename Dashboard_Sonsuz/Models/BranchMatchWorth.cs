using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class BranchMatchWorth
    {
        [Key]
        public long worthId { get; set; }

        public double amount { get; set; }

        public string paymentType { get; set; }

        public long matchId { get; set; }
        public MatchHistory match { get; set; }


    }
}
