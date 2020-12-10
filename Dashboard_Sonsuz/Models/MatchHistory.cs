using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class MatchHistory
    {
        [Key]
        public long matchId { get; set; }

        public long winnerTeam { get; set; }
        public Team winner { get; set; }

        public long loserTeam { get; set; }
        public Team loser { get; set; }

        public long areaId { get; set; }
        public AreaInfo area { get; set; }

        public string date { get; set; }

        public string score { get; set; }

        public double clock { get; set; }

        public int appointmentType { get; set; }


    }
}
