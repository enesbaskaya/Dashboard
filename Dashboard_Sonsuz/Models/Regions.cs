using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Dashboard_Sonsuz.Models
{
    public class Regions
    {
        [Key]
        public long regionId { get; set; }

        public string regionName { get; set; }

    }
}
