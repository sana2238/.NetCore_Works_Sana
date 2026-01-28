using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class DoctorAvailabilityDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
