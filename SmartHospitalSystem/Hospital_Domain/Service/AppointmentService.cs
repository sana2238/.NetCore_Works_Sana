using Hospital_Domain.Data;
using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using Microsoft.EntityFrameworkCore;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;
    private readonly AppDbContext _context;

    public AppointmentService(IAppointmentRepository repo,AppDbContext appDbContext)
    {
        _repo = repo;
        _context = appDbContext;
    }
    public List<Appointment> GetUpcomingAppointments(Guid doctorId)
    {
        return _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId &&
                        a.AppointmentDate >= DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .Take(5)
            .ToList();
    }

    public IEnumerable<Appointment> GetDoctorAppointments(Guid doctorId)
    {
        return _repo.GetByDoctorId(doctorId);
    }
    public IEnumerable<Appointment> GetAppointmentsByDoctor(Guid doctorId)
    {
        return _repo.GetAppointmentsByDoctor(doctorId);
    }
    public Appointment GetAppointmentWithDetails(Guid appointmentId)
    {
        return _repo.GetAppointmentWithDetails(appointmentId);
    }
    public void UpdateStatus(Guid appointmentId, AppointmentStatus status)
    {
        var appointment = _repo.GetById(appointmentId);
        appointment.Status = status;
        _repo.Update(appointment);
    }
    public void BookAppointment(AppointmentDto dto)
    {
        if (!_repo.IsDoctorAvailable(dto.DoctorId, dto.AppointmentDate))
            throw new Exception("Doctor not available at selected time.");

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            Symptoms = dto.Symptoms,
            Status = AppointmentStatus.Pending
        };

        _repo.AddAppointment(appointment);
    }
    public IEnumerable<Appointment> GetAppointmentsByPatient(Guid patientId)
    {
        return _repo.GetAppointmentsByPatient(patientId);
    }

    public void GetAppointment(AppointmentBookingDto dto, Guid patientId)
    {
        var appointment = new Appointment
        {
            PatientId = patientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            Symptoms = dto.Symptoms
        };

        _repo.AddAppointment(appointment);
    }
}
