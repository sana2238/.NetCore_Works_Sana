using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class PrescriptionDto
    {
        [Required]
        public Guid AppointmentId { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Medicines { get; set; }

        public string Notes { get; set; }

        // 👇 Display-only fields
        public string PatientName { get; set; }
        public string Symptoms { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; }
        public List<Prescription> PrescriptionHistory { get; set; }
    }
}
