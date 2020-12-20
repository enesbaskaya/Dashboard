using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class TeamData
    {

        [Key]
        public long dataId { get; set; }
        public int wins { get; set; }
        public int loses { get; set; }
        public int draws { get; set; }
        public long teamId { get; set; }
        public Team team { get; set; }

    }
}
