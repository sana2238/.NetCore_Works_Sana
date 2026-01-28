using AutoMapper;
using Hospital_Domain.Dtos;
using Hospital_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDoctorDto, Doctor>();
            CreateMap<PatientRegisterDto, Patient>();
            CreateMap<Doctor, DoctorProfileDto>()
    .ForMember(dest => dest.ProfileImagePath,
               opt => opt.MapFrom(src => src.ProfileImagePath));
        }

    }
}
