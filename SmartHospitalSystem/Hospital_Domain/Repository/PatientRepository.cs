using Hospital_Domain.Data;
using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public class PatientRepository:IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
        public bool EmailExists(string email)
        {
            return _context.Patients.Any(x => x.Email == email);
        }
        public Patient GetByEmail(string email)
        {
            return _context.Patients.FirstOrDefault(x => x.Email == email && x.IsActive);
        }
        public Patient GetById(Guid patientId)
        {
            return _context.Patients.FirstOrDefault(p => p.PatientId == patientId);
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

    }
}
