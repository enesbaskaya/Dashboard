using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class AdminNotification
    {

        [Key]
        public long adminNotificationId { get; set; }

        public string content { get; set; }
        public string header { get; set; }
        public string date { get; set; }

        public bool active { get; set; }

        public long notificationTypeId { get; set; }

        public NotificationTypes notificationType { get; set; }

    }
}
