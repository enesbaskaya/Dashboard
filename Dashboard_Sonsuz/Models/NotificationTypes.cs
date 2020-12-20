using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class NotificationTypes
    {
        [Key]
        public long notificationTypeId { get; set; }

        public string typeName { get; set; }

    }
}
