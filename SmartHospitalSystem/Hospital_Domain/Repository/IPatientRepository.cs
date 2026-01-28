using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);
        bool EmailExists(string email);
        Patient GetByEmail(string email);
        Patient GetById(Guid patientId);
        void UpdatePatient(Patient patient);
    }
}
