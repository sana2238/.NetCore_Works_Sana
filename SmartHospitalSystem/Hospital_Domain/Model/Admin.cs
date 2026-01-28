using Hospital_Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Model
{
    public class Admin
    {
        public Guid AdminId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.Admin;
    }
}
