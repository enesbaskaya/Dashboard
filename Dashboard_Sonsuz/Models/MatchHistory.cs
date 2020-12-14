using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class MatchHistory
    {
        [Key]
        public long matchId { get; set; }

        public long winnerTeamId { get; set; }
        public Team winnerTeam { get; set; }

        public long loserTeamId { get; set; }
        public Team loserTeam { get; set; }

        public long areaId { get; set; }
        public AreaInfo area { get; set; }

        public string date { get; set; }

        public string score { get; set; }

        public double clock { get; set; }

        public double amount { get; set; }

        public long paymentMethodId { get; set; }
        public PaymentMethods paymentMethod { get; set; }

        public long appointmentTypeId { get; set; }
        public AppointmentTypes appointmentType { get; set; }


    }
}
