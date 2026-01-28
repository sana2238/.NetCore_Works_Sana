using Hospital_Domain.Dtos;
using Hospital_Domain.Model;
using Hospital_Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IDoctorAvailabilityRepository _repo;
        public DoctorAvailabilityService(IDoctorAvailabilityRepository repo)
        {
            _repo = repo;
        }
        public List<DoctorAvailabilityDto> GetAvailability(Guid doctorId)
        {
            return _repo.GetByDoctor(doctorId)
            .Select(x => new DoctorAvailabilityDto
            {
                DayOfWeek = x.DayOfWeek,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                IsAvailable = x.IsAvailable
            }).ToList();
        }
        public void SaveAvailability(Guid doctorId, List<DoctorAvailabilityDto> dto)
        {
            var list = dto.Select(x => new DoctorAvailability
            {
                DoctorId = doctorId,
                DayOfWeek = x.DayOfWeek,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                IsAvailable = x.IsAvailable
            }).ToList();


            _repo.Save(doctorId, list);
        }
    }
}
