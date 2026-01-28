using Hospital_Domain.Model;

public interface IDoctorRepository
{
    //Doctor GetByEmail(string email);
    Doctor GetById(Guid doctorId);
    void Update(Doctor doctor);
    void Save();
    void Delete(Doctor doctor);
    IEnumerable<Doctor> GetAllDoctors();
    void Add(Prescription prescription);
    List<string> GetSpecializations();
    List<Doctor> GetDoctorsBySpecialization(string specialization);
    

}
