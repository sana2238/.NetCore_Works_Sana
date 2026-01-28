using Hospital_Domain.Data;
using Hospital_Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Repository
{
    public class PrescriptionRepository :IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Prescription> GetByAppointmentId(Guid appointmentId)
        {
            return _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .OrderByDescending(p => p.CreatedOn)
                .AsNoTracking()
                .ToList();
        }
        public void Add(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
        }
    }
}
