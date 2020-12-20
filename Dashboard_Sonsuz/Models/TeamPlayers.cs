using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class TeamPlayers
    {
        [Key]
        public long teamPlayersId { get; set; }
        public long userId { get; set; }
        public User user { get; set; }
        public long teamId { get; set; }
        public Team team { get; set; }
    }
}
