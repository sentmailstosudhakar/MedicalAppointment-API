using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MedicalAppointment_API.DataLayer.Models;

namespace MedicalAppointment_API.DataLayer.Contexts
{
    public class MedicalAppointmentContext : DbContext
    {
        public MedicalAppointmentContext(DbContextOptions<MedicalAppointmentContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

    }
}
