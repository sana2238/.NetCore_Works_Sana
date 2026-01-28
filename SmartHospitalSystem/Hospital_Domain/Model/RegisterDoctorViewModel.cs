using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SmartHospitalSystem.ViewModels
{
    public class RegisterDoctorViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Specialization { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }

        // ✅ NEW
        [Required]
        public DateOnly AvailableFromDate { get; set; }

        [Required]
        public DateOnly AvailableToDate { get; set; }

        [Required]
        public TimeOnly AvailableFromTime { get; set; }

        [Required]
        public TimeOnly AvailableToTime { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
