using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using MedicalAppointment_API.DataLayer.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace MedicalAppointment_API.Auth
{
    public class TokenHelper : ITokenHelper
    {
        public IConfiguration configuration { get; set; }
        public TokenHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            SecurityTokenDescriptor descriptor = GetSecurityTokenDescriptor(user);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(User user)
        {
            double JWTExpireInHours = double.Parse(configuration["JWT:expireInHours"]);
            var JWTKey = Encoding.ASCII.GetBytes(configuration["JWT:Key"]);
            return new SecurityTokenDescriptor
            {
                Subject = this.GetClaimsIdentity(user),
                Expires = DateTime.UtcNow.AddHours(JWTExpireInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(JWTKey), SecurityAlgorithms.HmacSha256Signature)
            };
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            return new ClaimsIdentity(new Claim[]
              {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, user.userRole.name),
              });
        }
    }
}
