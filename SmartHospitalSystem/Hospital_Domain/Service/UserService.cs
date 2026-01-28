using AutoMapper;
using Hospital_Domain.Dtos;
//using Hospital_Domain.Domain.Entities;
using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Extension;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using Hospital_Domain.Service;
//using Hospital_Domain.Extension;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public bool EmailExists(string email)
    {
        return _repo.EmailExists(email);
    }
    public Doctor RegisterDoctor(RegisterDoctorDto dto, string imagePath)
    {
        if (_repo.ExistsByEmail(dto.Email))
            throw new Exception("Doctor with this email already exists");

        var doctor = _mapper.Map<Doctor>(dto);

        doctor.DoctorId = Guid.NewGuid();
        doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        doctor.ProfileImagePath = imagePath;
        doctor.Role = Role.Doctor;
        doctor.AvailableFromDate = dto.AvailableFromDate;
        doctor.AvailableToDate = dto.AvailableToDate;
        doctor.AvailableFromTime = dto.AvailableFromTime;
        doctor.AvailableToTime = dto.AvailableToTime;
        _repo.AddDoctor(doctor);
        _repo.Save();

        return doctor;
    }

    public Doctor AuthenticateDoctor(LoginDto dto)
    {
        var doctor = _repo.GetDoctorByEmail(dto.Email);
        if (doctor == null)
            return null;

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, doctor.PasswordHash);
        if (!isValid)
            return null;

        // 🔥 Upgrade legacy password if needed
        if (!doctor.PasswordHash.StartsWith("$2"))
        {
            doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            _repo.Save();
        }

        return doctor;
    }

    public Doctor GetDoctorByEmail(string email)
    {
        return _repo.GetDoctorByEmail(email);
    }


}
