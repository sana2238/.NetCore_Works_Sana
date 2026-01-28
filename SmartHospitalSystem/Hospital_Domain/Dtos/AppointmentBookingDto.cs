using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class AppointmentBookingDto
    {
        public string Specialization { get; set; }
        public Guid DoctorId { get; set; }
        public DateOnly? AvailableFromDate { get; set; }
        public DateOnly? AvailableToDate { get; set; }
        public TimeOnly? AvailableFromTime { get; set; }
        public TimeOnly? AvailableToTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Symptoms { get; set; }
    }
}
