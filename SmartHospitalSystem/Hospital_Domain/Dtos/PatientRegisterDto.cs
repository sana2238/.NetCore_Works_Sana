using Hospital_Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Dtos
{
    public class PatientRegisterDto
    {
        public string Name { get; set; }   
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Place { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }


    }
}
