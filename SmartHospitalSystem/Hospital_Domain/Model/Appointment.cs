using Hospital_Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Model
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; } = Guid.NewGuid();
        public DateTime AppointmentDate { get; set; }
        public string Symptoms { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
