    using Hospital_Domain.Enum;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
 
    namespace Hospital_Domain.Model   
    {
        public class Doctor
        {
        [Key]
        public Guid DoctorId { get; set; }
      

        public string Name { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }

        // ✅ NEW
        public DateOnly? AvailableFromDate { get; set; }
        public DateOnly? AvailableToDate { get; set; }
        public TimeOnly? AvailableFromTime { get; set; }
        public TimeOnly? AvailableToTime { get; set; }

        public string ProfileImagePath { get; set; }
        public Role Role { get; set; } = Role.Doctor;

        public ICollection<Appointment> Appointments { get; set; }
    }

    }

