using Hospital_Domain.Data;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using Microsoft.EntityFrameworkCore;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }
    public Appointment GetAppointmentWithDetails(Guid appointmentId)
    {
        return _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefault(a => a.AppointmentId == appointmentId);
    }
    public IEnumerable<Appointment> GetByDoctorId(Guid doctorId)
    {
        return _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId)
            .ToList();
    }
    public IEnumerable<Appointment> GetAppointmentsByDoctor(Guid doctorId)
    {
        return _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToList();
    }
    public Appointment GetById(Guid id)
    {
        return _context.Appointments.FirstOrDefault(a => a.AppointmentId == id);
    }

    public void Update(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        _context.SaveChanges();
    }
    public void AddAppointment(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        _context.SaveChanges();
    }

    public bool IsDoctorAvailable(Guid doctorId, DateTime appointmentDate)
    {
        return !_context.Appointments.Any(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate == appointmentDate);
    }
    public IEnumerable<Appointment> GetAppointmentsByPatient(Guid patientId)
    {
        return _context.Appointments
            .Include(a => a.Doctor)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToList();
    }
}
