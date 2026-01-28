using Hospital_Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hospital_Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> AdminDetails { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Email = "SmartAdmin@gmail.com",
                    PasswordHash = "SmartAdmin@123"
                }
            );

        }
    }
}
