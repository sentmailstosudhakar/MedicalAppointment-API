using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MedicalAppointment_API.DataLayer.Models;

namespace MedicalAppointment_API.Auth
{
    public interface ITokenHelper
    {
        public string GenerateToken(User user);
    }
}
