using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class Branch 
    {
        [Key]
        public long branchId { get; set; }

        public string admin { get; set; }

        public string taxNumber { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public string identityNumber { get; set; }

        public string openDays { get; set; }

        public bool shoesRent { get; set; }
        public bool market { get; set; }
        public bool catering { get; set; }
        public bool shower { get; set; }

        public string paymentMethods { get; set; }

        public string branchType { get; set; }

        public bool isActive { get; set; }
        public string registerDate { get; set; }

        public long contactId { get; set; }
        public ContactInfo contact { get; set; }


    }
}
