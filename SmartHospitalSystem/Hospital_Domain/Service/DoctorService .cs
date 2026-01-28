using Hospital_Domain.Data;
using Hospital_Domain.Dtos;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repo;
    private readonly IPrescriptionRepository _prescriptionRepo;
    private readonly AppDbContext _dbcontext;
    public DoctorService(IDoctorRepository repo, IPrescriptionRepository prescriptionRepo,AppDbContext appDbContext)
    {
        _repo = repo;
        _prescriptionRepo = prescriptionRepo;
        _dbcontext = appDbContext;
    }

    public DoctorProfileDto GetDoctorProfile(Guid doctorId)
    {
        var doctor = _repo.GetById(doctorId);
        if (doctor == null) return null;

        return new DoctorProfileDto
        {
            Name = doctor.Name,
            Email = doctor.Email,
            Specialization = doctor.Specialization
        };
    }

    public void UpdateDoctorProfile(Guid doctorId, DoctorProfileDto dto)
    {
        var doctor = _repo.GetById(doctorId);

        if (doctor == null)
            throw new Exception("Doctor not found");

        doctor.Name = dto.Name;
        doctor.Specialization = dto.Specialization;

        // 🚫 DO NOT touch PasswordHash here

        _repo.Update(doctor);
    }
    public void DeleteDoctor(Guid doctorId)
    {
        var doctor = _repo.GetById(doctorId);

        if (doctor == null)
            throw new Exception("Doctor not found");

        _repo.Delete(doctor);
        _repo.Save();
    }
    public bool ChangePassword(Guid doctorId, ChangePasswordDto dto)
    {
        var doctor = _repo.GetById(doctorId);
        if (doctor == null)
            return false;

        bool passwordValid;

        // 🔐 If already BCrypt
        if (!string.IsNullOrEmpty(doctor.PasswordHash) &&
            doctor.PasswordHash.StartsWith("$2"))
        {
            passwordValid = BCrypt.Net.BCrypt.Verify(
                dto.CurrentPassword,
                doctor.PasswordHash
            );
        }
        else
        {
            // ⚠️ Legacy password (plain text / old hash)
            passwordValid = dto.CurrentPassword == doctor.PasswordHash;
        }

        if (!passwordValid)
            return false;

        // ✅ ALWAYS store new password as BCrypt
        doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        _repo.Save();
        return true;
    }


    public void AddPrescription(PrescriptionDto dto)
    {
        var prescription = new Prescription
        {
            PrescriptionId = Guid.NewGuid(),
            AppointmentId = dto.AppointmentId,
            Diagnosis = dto.Diagnosis,
            Medicines = dto.Medicines,
            Notes = dto.Notes,
            CreatedOn = DateTime.Now
        };

        _prescriptionRepo.Add(prescription);
    }

    public IEnumerable<Doctor> GetAllDoctors()
    {
        return _repo.GetAllDoctors();
    }
    public List<string> GetSpecializations()
    {
        return _repo.GetSpecializations();
    }
    public int GetPrescriptionCount(Guid doctorId)
    {
        return _dbcontext.Prescriptions
            .Include(p => p.Appointment)
            .Count(p => p.Appointment.DoctorId == doctorId);
    }


    public List<Doctor> GetDoctors(string specialization)
    {
        return _repo.GetDoctorsBySpecialization(specialization);
    }
    public int GetPatientCount(Guid doctorId)
    {
        return _dbcontext.Appointments
            .Where(a => a.DoctorId == doctorId && a.PatientId != null)
            .Select(a => a.PatientId)
            .Distinct()
            .Count();
    }

    public Doctor GetDoctor(Guid doctorId)
    {
        return _repo.GetById(doctorId);
    }
}
