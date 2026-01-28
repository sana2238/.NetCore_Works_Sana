using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Domain.Dtos
{
    public class RegisterDoctorDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }
        public string Password { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }

        // ✅ NEW
        public DateOnly AvailableFromDate { get; set; }
        public DateOnly AvailableToDate { get; set; }
        public TimeOnly AvailableFromTime { get; set; }
        public TimeOnly AvailableToTime { get; set; }


    }
}
