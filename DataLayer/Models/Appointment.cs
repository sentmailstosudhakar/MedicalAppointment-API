using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAppointment_API.DataLayer.Models
{
    public class Appointment
    {
        public int id { get; set; }
        public DateTime scheduleOn { get; set; }
        public virtual User user { get; set; }
    }
}
