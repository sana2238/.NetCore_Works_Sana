using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public interface IPrescriptionRepository
    {
        List<Prescription> GetByAppointmentId(Guid appointmentId);

        void Add(Prescription prescription);
    }
}
