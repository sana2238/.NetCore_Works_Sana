using Hospital_Domain.Dtos;
using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public interface IUserService
    {
        Doctor RegisterDoctor(RegisterDoctorDto dto, string imagePath);

        bool EmailExists(string email);
        Doctor AuthenticateDoctor(LoginDto dto);

        Doctor GetDoctorByEmail(string email);

    }
}
