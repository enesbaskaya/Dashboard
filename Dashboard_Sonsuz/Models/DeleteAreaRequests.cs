﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DeleteAreaRequests
    {
        [Key]
        public long deleteRequestId { get; set; }

        public long areaId { get; set; }
        public AreaInfo area { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string date { get; set; }

        public bool isActive { get; set; }




    }
}
