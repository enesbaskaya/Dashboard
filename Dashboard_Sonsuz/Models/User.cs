using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class User
    {
        [Key]
        public long userId { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string dateOfBirth { get; set; }
        public string position { get; set; }
        public string footType { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public long contactId { get; set; }

        public long statusId { get; set; }
        public Status status { get; set; }

        public string registerDate { get; set; }
        public ContactInfo contact { get; set; }



    }
}
