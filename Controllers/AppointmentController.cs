using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MedicalAppointment_API.DataLayer.Generics;
using MedicalAppointment_API.DataLayer.Models;
using MedicalAppointment_API.ResponseModel;

namespace MedicalAppointment_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        public IGenericRepository<Appointment> appointmentRepository;
        public IGenericRepository<User> userRepository;
        public AppointmentController(IGenericRepository<Appointment> appointmentRepository, IGenericRepository<User> userRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public IActionResult scheduleAppointment(string username)
        {
            Appointment appointment = new Appointment();
            appointment.scheduleOn = DateTime.Now;
            appointment.user = this.userRepository.FindBy(user => user.userName == username, user=> user).FirstOrDefault();
            appointmentRepository.Insert(appointment);
            appointmentRepository.Save();
            return Ok(new BaseResponseModel<string>() { Code = StatusCodes.Status200OK, Message = "Appointment Scheduled"});
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public IActionResult GetAppointments()
        {
            List<Appointment> appointments = this.appointmentRepository.GetAll().ToList();
            return Ok(new BaseResponseModel<List<Appointment>>() { Code = StatusCodes.Status200OK, Message = "Appointments Fetched Successfully", Data = appointments});
        }
    }
}
