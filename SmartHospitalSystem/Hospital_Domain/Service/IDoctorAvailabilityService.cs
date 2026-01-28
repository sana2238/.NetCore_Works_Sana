using Hospital_Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public interface IDoctorAvailabilityService
    {
        List<DoctorAvailabilityDto> GetAvailability(Guid doctorId);
        void SaveAvailability(Guid doctorId, List<DoctorAvailabilityDto> dto);
    }
}
