using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Dashboard_Sonsuz.Models
{
    public class TransActions
    {
        [Key]
        public long transId { get; set; }

        public long cardId { get; set; }
        public BranchCards card { get; set; }

        public string date { get; set; }

        public double amount { get; set; }

        public bool checkActive { get; set; }



    }
}
