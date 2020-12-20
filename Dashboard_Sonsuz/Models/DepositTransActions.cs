using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DepositTransActions
    {

        [Key]
        public long transId { get; set; }
        public string date { get; set; }
        public double amount { get; set; }
        public long statusId { get; set; }
        public Status status { get; set; }
        public long branchId { get; set; }
        public Branch branch { get; set; }

    }
}
