using Hospital_Domain.Dtos;
using Hospital_Domain.Model;

public interface IDoctorService
{
    //Doctor GetCurrentDoctor(string email);
    IEnumerable<Doctor> GetAllDoctors();
    void AddPrescription(PrescriptionDto dto);
    DoctorProfileDto GetDoctorProfile(Guid doctorId);
    void UpdateDoctorProfile(Guid doctorId, DoctorProfileDto dto);
    bool ChangePassword(Guid doctorId, ChangePasswordDto dto);
    void DeleteDoctor(Guid doctorId);
    List<string> GetSpecializations();
    List<Doctor> GetDoctors(string specialization);
    Doctor GetDoctor(Guid doctorId);
    int GetPatientCount(Guid doctorId);
    int GetPrescriptionCount(Guid doctorId);


}
