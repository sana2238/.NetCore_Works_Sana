using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public interface IDoctorAvailabilityRepository
    {
        List<DoctorAvailability> GetByDoctor(Guid doctorId);
        void Save(Guid doctorId, List<DoctorAvailability> availability);
    }
}
