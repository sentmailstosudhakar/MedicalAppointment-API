using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using MedicalAppointment_API.DataLayer.Models;
using MedicalAppointment_API.DataLayer.Generics;
using MedicalAppointment_API.Auth;
using MedicalAppointment_API.ResponseModel;
using MedicalAppointment_API.Model;

namespace MedicalAppointment_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private IGenericRepository<User> userRepository { get; set; }
        private ITokenHelper tokenHandler { get; set; }

        public LoginController(IGenericRepository<User> userRepository, ITokenHelper tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate(InputUser inputUser)
        {
            try
            {
                User user = userRepository.FindBy(user => user.userName.ToLower() == inputUser.username.ToLower() && user.password == inputUser.password, user => user).FirstOrDefault();
                if (user == null)
                    return Ok(new BaseResponseModel<string>() { Code = StatusCodes.Status401Unauthorized, Message = "Invalid Credential", Data = null });
                var token = tokenHandler.GenerateToken(user);
                inputUser.password = "";
                inputUser.role = user.userRole.name;
                inputUser.token = token;
                return Ok(new BaseResponseModel<InputUser> { Code = StatusCodes.Status200OK, Message = "User Validated", Data = inputUser });
            }
            catch(Exception ex)
            {
                return Ok(new BaseResponseModel<string> { Code = StatusCodes.Status500InternalServerError, Message = "Something went wrong", Data = ex.Message });
            }
        }

    }
}