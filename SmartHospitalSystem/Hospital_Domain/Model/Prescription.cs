using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Model
{
    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; } = Guid.NewGuid();

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string Diagnosis { get; set; }
        public string Medicines { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
