using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class BranchStars
    {
        [Key]
        public long starId { get; set; }

        public long branchId { get; set; }
        public Branch branch { get; set; }

        public int point { get; set; }

        public string year { get; set; }

        public int count { get; set; }

    }
}
