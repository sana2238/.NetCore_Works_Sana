using Hospital_Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Model
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string BloodGroup { get; set; }
        public string Place { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; } = Role.Patient;
    }
}
