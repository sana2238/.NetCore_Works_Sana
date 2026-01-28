using Hospital_Domain.Data;
using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public class DoctorAvailabilityRepository : IDoctorAvailabilityRepository
    {
        private readonly AppDbContext _context;
        public DoctorAvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<DoctorAvailability> GetByDoctor(Guid doctorId)
        {
            return _context.DoctorAvailabilities
            .Where(x => x.DoctorId == doctorId)
            .ToList();
        }
        public void Save(Guid doctorId, List<DoctorAvailability> availability)
        {
            var old = _context.DoctorAvailabilities.Where(x => x.DoctorId == doctorId);
            _context.DoctorAvailabilities.RemoveRange(old);
            _context.DoctorAvailabilities.AddRange(availability);
            _context.SaveChanges();
        }
    }
}
