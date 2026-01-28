using Hospital_Domain.Dtos;
using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public interface IPatientService
    {
        void RegisterPatient(PatientRegisterDto dto);
        bool EmailExists(string email);
        Patient Login(LoginDto dto);
        Patient GetPatientByEmail(string email);
        void UpdatePatient(Guid patientId, PatientRegisterDto dto);
    }
}
