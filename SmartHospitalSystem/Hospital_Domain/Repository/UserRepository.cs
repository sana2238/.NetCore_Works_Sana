using Hospital_Domain.Data;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
//using HospitalManagementSystem.Data;
//using HospitalManagementSystem.Domain.Entities;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Doctor GetDoctorByEmail(string email)
    {
        return _context.Doctors.FirstOrDefault(d => d.Email == email);
    }
    public bool EmailExists(string email)
    {
        return _context.Doctors
            .Any(d => d.Email.ToLower() == email.ToLower());
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public bool ExistsByEmail(string email)
    {
        return _context.Doctors.Any(d => d.Email == email);
    }

    public void AddDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
    }
}
