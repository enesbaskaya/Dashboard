using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard_Sonsuz.Models
{
    public class City
    {
        [Key]
        public long cityId { get; set; }

        public string cityName { get; set; }

        public long regionId { get; set; }
        public Regions region { get; set; }


    }
}
