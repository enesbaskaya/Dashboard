using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class ContactInfo
    {
        [Key]
        public long contactId { get; set; }

        public string telephone { get; set; }
        public string address { get; set; }
        public string mail { get; set; }
        public long districtId { get; set; }
        public Districts district { get; set; }


    }
}
