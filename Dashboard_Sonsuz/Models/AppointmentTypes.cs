using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class AppointmentTypes
    {
        [Key]
        public long appointmentTypeId { get; set; }

        public string appointmentName { get; set; }
    }
}
