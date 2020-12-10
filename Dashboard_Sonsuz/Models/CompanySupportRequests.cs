using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class CompanySupportRequests
    {
        [Key]
        public long requestId { get; set; }

        public string header { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public bool isActive { get; set; }
        public long branchId { get; set; }
        public Branch branch { get; set; }


    }
}
