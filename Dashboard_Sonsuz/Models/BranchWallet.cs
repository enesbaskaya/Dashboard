using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class BranchWallet
    {
        [Key]
        public long walletId { get; set; }

        public double balance { get; set; }
        public double debt { get; set; }

        public long branchId { get; set; }
        public Branch branch { get; set; }


    }
}
