using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class Districts
    {
        [Key]
        public long districtId { get; set; }

        public string districtName { get; set; }

        public long cityId { get; set; }
        public City city { get; set; }


    }
}
