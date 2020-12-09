using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class BranchCards
    {
        [Key]
        public long cardId { get; set; }

        public string iban { get; set; }
        public string cardOwner { get; set; }
        public string bankName { get; set; }
        public long branchId { get; set; }
        public Branch branch { get; set; }


    }
}
