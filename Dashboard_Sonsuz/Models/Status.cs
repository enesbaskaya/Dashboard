using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class Status
    {
        [Key]
        public long statusId { get; set; }
        public string name { get; set; }
    }
}
