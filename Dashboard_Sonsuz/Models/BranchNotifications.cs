using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class BranchNotifications
    {
        [Key]
        public long notificationId { get; set; }

        public string content { get; set; }
        public string header { get; set; }
        public string sender { get; set; }
        public bool isRead { get; set; }
        public string date { get; set; }

        public long branchId { get; set; }
        public Branch branch { get; set; }


    }
}
