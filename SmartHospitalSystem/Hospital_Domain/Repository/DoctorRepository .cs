using Hospital_Domain.Data;
using Hospital_Domain.Model;
using Microsoft.EntityFrameworkCore;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        _context.SaveChanges();
    }
    public IEnumerable<Doctor> GetAllDoctors()
    {
        return _context.Doctors
                   .OrderBy(d => d.Name)
                   .ToList();
    }
    public Doctor GetById(Guid doctorId)
    {
        return _context.Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public void Delete(Doctor doctor)
    {
        _context.Doctors.Remove(doctor);
    }
    public void Update(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        _context.SaveChanges();
    }
    public List<string> GetSpecializations()
    {
        return _context.Doctors
            .Select(d => d.Specialization)
            .Distinct()
            .ToList();
    }

    public List<Doctor> GetDoctorsBySpecialization(string specialization)
    {
        return _context.Doctors
            .Where(d => d.Specialization == specialization)
            .ToList();
    }
}
