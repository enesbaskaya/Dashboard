using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class PaymentMethods
    {

        [Key]
        public long paymentMethodId { get; set; }

        public string paymentMethodName { get; set; }

    }
}
