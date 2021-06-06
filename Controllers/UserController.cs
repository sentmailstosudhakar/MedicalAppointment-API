using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using MedicalAppointment_API.DataLayer.Models;
using MedicalAppointment_API.DataLayer.Generics;
using MedicalAppointment_API.ResponseModel;
using MedicalAppointment_API.Model;

namespace MedicalAppointment_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<UserRole> userRoleRepository;

        public UserController(IGenericRepository<User> userRepository, IGenericRepository<UserRole> userRoleRepository)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser(InputUser inputUser)
        {
            try
            {
                UserRole userRole = userRoleRepository.FindBy(role => role.name.ToLower() == inputUser.role.ToLower(), role => role).FirstOrDefault();
                if (userRole == null)
                    return Ok(new BaseResponseModel<string>() { Code = StatusCodes.Status404NotFound, Message = "Role Not Found", Data = null });
                User registeredUser = userRepository.FindBy(user => user.userName.ToLower() == inputUser.username.ToLower(), user => user).FirstOrDefault();
                if (registeredUser != null)
                {
                    return Ok(new BaseResponseModel<string>() { Code = StatusCodes.Status200OK, Message = "Username already taken." });
                }
                User user = new User();
                user.userName = inputUser.username;
                user.password = inputUser.password;
                user.userRole = userRole;
                userRepository.Insert(user);
                userRepository.Save();
                return Ok(new BaseResponseModel<string>() { Code = StatusCodes.Status200OK, Message = "User Created Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel<string> { Code = StatusCodes.Status500InternalServerError, Message = "Something went wrong", Data = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDoctorsPatients()
        {
            List<User> doctorsPatients = userRepository.FindBy(user => user.userRole.name == "Doctor" || user.userRole.name == "Patient", user => user).OrderBy(user => user.userRole.name).ToList();
            return Ok(new BaseResponseModel<List<User>>() { Code = StatusCodes.Status200OK, Message = "Doctors and Patients List", Data = doctorsPatients });
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public IActionResult GetPatients()
        {
            var patients = userRepository.FindBy(user => user.userRole.name == "Patient", user => user).ToList();
            return Ok(new BaseResponseModel<List<User>>() { Code = StatusCodes.Status200OK, Message = "Patients List", Data = patients });
        }
    }
}
