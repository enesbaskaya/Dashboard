using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
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


        public BranchCards(long cardId, string iban, string cardOwner, string bankName, long branchId)
        {
            this.cardId = cardId;
            this.iban = iban;
            this.cardOwner = cardOwner;
            this.bankName = bankName;
            this.branchId = branchId;
        }

    }
}
