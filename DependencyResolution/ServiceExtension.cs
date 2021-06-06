using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using MedicalAppointment_API.DataLayer.Contexts;
using MedicalAppointment_API.DataLayer.Generics;
using MedicalAppointment_API.DataLayer.Models;
using MedicalAppointment_API.Auth;



namespace MedicalAppointment_API.DependencyResolution
{
    public static class ServiceExtension
    {

        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configure)
        {
            #region DB Connection
            services.AddDbContext<MedicalAppointmentContext>(
                options => options.UseLazyLoadingProxies().UseSqlServer(configure.GetConnectionString("MedicalAppointmentConnection")));
            #endregion

            #region Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configure["JWT:Key"]))
                };
            });
            #endregion

            #region Token Dependency
            services.AddScoped<ITokenHelper, TokenHelper>();
            #endregion

            #region Repository Dependency
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<UserRole>, GenericRepository<UserRole>>();
            services.AddScoped<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
            #endregion

            #region Cross Origin Requests
            services.AddCors(cors =>
            {
                cors.AddPolicy("CorsPolicy", policy =>
                 {
                     policy.AllowAnyMethod();
                     policy.AllowAnyHeader();
                     policy.AllowCredentials();
                     policy.WithOrigins(configure.GetSection("AllowedOrigins").Value.Split(',', StringSplitOptions.RemoveEmptyEntries));
                 });
            });
            #endregion

            return services;
        }
    }
}
