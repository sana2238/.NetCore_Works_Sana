using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public interface IUserRepository
    {
        bool EmailExists(string email);
        void Save();
        bool ExistsByEmail(string email);
        Doctor GetDoctorByEmail(string email);
        void AddDoctor(Doctor doctor);
    }
}
