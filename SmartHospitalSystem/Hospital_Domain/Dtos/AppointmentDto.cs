using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class AppointmentDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Specialization { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Symptoms { get; set; }
    }
}
