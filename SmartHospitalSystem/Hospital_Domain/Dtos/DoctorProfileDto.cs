using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class DoctorProfileDto
    {
        public Guid DoctorId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Specialization { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }
        public string ProfileImagePath { get; set; }

        public DateOnly? AvailableFromDate { get; set; }
        public DateOnly? AvailableToDate { get; set; }
        public TimeOnly? AvailableFromTime { get; set; }
        public TimeOnly? AvailableToTime { get; set; }
    }
}
