using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Model;

public interface IAppointmentService
{
    IEnumerable<Appointment> GetDoctorAppointments(Guid doctorId);
    void UpdateStatus(Guid appointmentId, AppointmentStatus status);
    void BookAppointment(AppointmentDto dto);
    IEnumerable<Appointment> GetAppointmentsByPatient(Guid patientId);
    IEnumerable<Appointment> GetAppointmentsByDoctor(Guid doctorId);
    Appointment GetAppointmentWithDetails(Guid appointmentId);
    void GetAppointment(AppointmentBookingDto dto, Guid patientId);
    List<Appointment> GetUpcomingAppointments(Guid doctorId);


}
