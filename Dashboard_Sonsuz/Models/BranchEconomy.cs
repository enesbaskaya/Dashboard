using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class BranchEconomy
    {
        [Key]
        public long branchEconomyId { get; set; }

        public string year { get; set; }

        public string month { get; set; }
        public double giro { get; set; }

        public long branchId { get; set; }
        public Branch branch { get; set; }


    }
}
