using Hospital_Domain.Data;
using Hospital_Domain.Email;  // <- this must match your EmailSettings class
using Hospital_Domain.Helper;
using Hospital_Domain.Repository;
using Hospital_Domain.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace SmartHospitalSystem.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
            services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();

            // Services
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IUserService, UserService>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
