using Hospital_Domain.Model;

public interface IAppointmentRepository
{
    IEnumerable<Appointment> GetByDoctorId(Guid doctorId);
    Appointment GetById(Guid id);
    void Update(Appointment appointment);
    void AddAppointment(Appointment appointment);
    bool IsDoctorAvailable(Guid doctorId, DateTime appointmentDate);
    IEnumerable<Appointment> GetAppointmentsByPatient(Guid patientId);
    IEnumerable<Appointment> GetAppointmentsByDoctor(Guid doctorId);
    Appointment GetAppointmentWithDetails(Guid appointmentId);
}
