using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAppointment_API.Model
{
    public class InputUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string? role { get; set; }
        public string? token { get; set; }
    }
}
