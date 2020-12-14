
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class BranchPaymentMethods
    {

        [Key]
        public long branchPaymentMethodId { get; set; }

        public long branchId { get; set; }
        public Branch branch { get; set; }

        public long paymentMethodId { get; set; }

        public PaymentMethods paymentMethod { get; set; }

    }
}
