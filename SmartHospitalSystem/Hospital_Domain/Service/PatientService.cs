using AutoMapper;
using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Extension;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public class PatientService:IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        public PatientService(IPatientRepository patientRepository , IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }
        public void RegisterPatient(PatientRegisterDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);

            patient.PasswordHash = PasswordHasher.Hash(dto.Password);
            patient.IsActive = true;
            patient.CreatedAt = DateTime.UtcNow;
            patient.Role = Role.Patient;

            _patientRepository.AddPatient(patient);
        }
        public bool EmailExists(string email)
        {
            return _patientRepository.EmailExists(email);
        }
        public Patient Login(LoginDto dto)
        {
            var patient = _patientRepository.GetByEmail(dto.Email);

            if (patient == null)
                return null;

            var hashedPassword = PasswordHasher.Hash(dto.Password);

            if (patient.PasswordHash != hashedPassword)
                return null;

            return patient;
        }
        public Patient GetPatientByEmail(string email)
        {
            return _patientRepository.GetByEmail(email);
        }
        public void UpdatePatient(Guid patientId, PatientRegisterDto dto)
        {
            var patient = _patientRepository.GetById(patientId);
            if (patient == null)
                throw new Exception("Patient not found");

            patient.Name = dto.Name;
            patient.Email = dto.Email;
            patient.Phone = dto.Phone;
            patient.Address = dto.Address;
            patient.Gender = dto.Gender;
            patient.Age = dto.Age;
            patient.BloodGroup = dto.BloodGroup;
            patient.Place = dto.Place;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                patient.PasswordHash = PasswordHasher.Hash(dto.Password);
            }

            _patientRepository.UpdatePatient(patient);
        }



    }
}
