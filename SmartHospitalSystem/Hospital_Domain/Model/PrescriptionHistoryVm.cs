using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Model
{
    public class PrescriptionHistoryVm
    {
        public Guid PrescriptionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Diagnosis { get; set; }
        public string Medicines { get; set; }
        public string Notes { get; set; }

        // Display-only
        public string PatientName { get; set; }
        public string AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Symptoms { get; set; }
    }
}
