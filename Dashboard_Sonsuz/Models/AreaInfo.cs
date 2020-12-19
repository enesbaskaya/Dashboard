using Dashboard.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class AreaInfo
    {
        [Key]
        public long areaId { get; set; }

        public string areaName { get; set; }

        public string ground { get; set; }

        public string heightWidth { get; set; }

        public string maxPlayer { get; set; }

        public string openCloseTime { get; set; }

        public bool recordingMatch { get; set; }

        public double price { get; set; }

        public string time { get; set; }

        public bool roof { get; set; }

        public string registerDate { get; set; }
        public long statusId { get; set; }
        public Status status { get; set; }
        public long branchId { get; set; }
        public Branch branch { get; set; }

    }
}
