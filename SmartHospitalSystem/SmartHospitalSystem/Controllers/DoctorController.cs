using Hospital_Domain.Data;
using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Model;
using Hospital_Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHospitalSystem.Extensions;
using System.Numerics;
using System.Security.Claims;

[SessionAuthorize("Doctor")]
public class DoctorController : Controller
{

    private readonly IDoctorService _doctorService;
    private readonly IAppointmentService _appointmentService;
    private readonly AppDbContext _context;
    private readonly IDoctorAvailabilityService _availabilityService;

    public DoctorController(
        IDoctorService doctorService,
        IAppointmentService appointmentService, AppDbContext appDbContext, IDoctorAvailabilityService availabilityService)
    {
        _doctorService = doctorService;
        _appointmentService = appointmentService;
        _context = appDbContext;
        _availabilityService = availabilityService;
    }
    [SessionAuthorize("Doctor")]
    public IActionResult Dashboard()
    {
        Guid doctorId = Guid.Parse(HttpContext.Session.GetString("DoctorId"));

        // ✅ Get logged-in doctor
        var doctor = _doctorService.GetDoctorProfile(doctorId);

        ViewBag.DoctorName = doctor.Name;

        ViewBag.ProfileImagePath = doctor.ProfileImagePath;

        ViewBag.PatientCount = _doctorService.GetPatientCount(doctorId);
        ViewBag.PrescriptionCount = _doctorService.GetPrescriptionCount(doctorId);

        var upcomingAppointments =
            _appointmentService.GetUpcomingAppointments(doctorId);

        ViewBag.UpcomingAppointments = upcomingAppointments;

        ViewBag.TodayAppointments =
            upcomingAppointments.Count(a =>
                a.AppointmentDate.Date == DateTime.Today);

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
   
    [SessionAuthorize("Doctor")]
    public IActionResult Appointments()
    {
        try
        {
            var doctorIdString = HttpContext.Session.GetString("DoctorId");

            if (string.IsNullOrEmpty(doctorIdString))
                return RedirectToAction("Login", "Account");

            Guid doctorId = Guid.Parse(doctorIdString);

            var appointments = _appointmentService
                .GetAppointmentsByDoctor(doctorId);

            return View(appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Unable to load appointments.";
            return RedirectToAction("Dashboard");
        }
    }

    [HttpPost]
    public IActionResult UpdateAppointmentStatus(Guid id, AppointmentStatus status)
    {
        _appointmentService.UpdateStatus(id, status);
        return RedirectToAction("Appointments");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddPrescription(PrescriptionDto model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _doctorService.AddPrescription(model);

        TempData["Success"] = "Prescription added successfully!";
        return RedirectToAction("Appointments");
    }
    [HttpGet]
    public IActionResult Prescription(Guid id)
    {
        var appointment = _appointmentService.GetAppointmentWithDetails(id);

        if (appointment == null)
        {
           
            return NotFound("Appointment not found.");
        }

        var dto = new PrescriptionDto
        {
            AppointmentId = appointment.AppointmentId,
            PatientName = appointment.Patient?.Name ?? "Unknown",
            Symptoms = appointment.Symptoms ?? "—",
            AppointmentDate = appointment.AppointmentDate,
            DoctorName = appointment.Doctor?.Name ?? "Unknown"
        };

        return View(dto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Prescription(PrescriptionDto model)
    {
       

        _doctorService.AddPrescription(model);

        return RedirectToAction("Appointments");
    }



    [HttpGet]
    public IActionResult Profile(Guid? id)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (string.IsNullOrEmpty(role))
            return RedirectToAction("Login", "Account");

        // 🔹 ADMIN → can edit ANY doctor
        if (role == "Admin")
        {
            if (id == null || id == Guid.Empty)
                return BadRequest("Doctor ID missing");

            var adminModel = _doctorService.GetDoctorProfile(id.Value);
            if (adminModel == null)
                return NotFound("Doctor not found");

            return View(adminModel);
        }

        // 🔹 DOCTOR → can edit ONLY own profile
        if (role == "Doctor")
        {
            var doctorIdString = HttpContext.Session.GetString("DoctorId");

            if (string.IsNullOrEmpty(doctorIdString))
                return RedirectToAction("Login", "Account");

            Guid doctorId = Guid.Parse(doctorIdString);

            var model = _doctorService.GetDoctorProfile(doctorId);
            if (model == null)
                return NotFound("Doctor not found");

            return View(model);
        }

        return RedirectToAction("Login", "Account");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(DoctorProfileDto dto)
    {
        try
        {
            var doctorIdString = HttpContext.Session.GetString("DoctorId");
            if (string.IsNullOrEmpty(doctorIdString))
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
                return View(dto);

            Guid doctorId = Guid.Parse(doctorIdString);

            _doctorService.UpdateDoctorProfile(doctorId, dto);

            HttpContext.Session.SetString("DoctorName", dto.Name);

            TempData["Success"] = "Profile updated successfully";
            return RedirectToAction("Profile"); // stay on same page
        }
        catch
        {
            TempData["Error"] = "Failed to update profile.";
            return View(dto);
        }
    }

    [SessionAuthorize("Doctor")]
    [HttpGet]
    public IActionResult PrescriptionHistory(Guid appointmentId)
    {
        if (appointmentId == Guid.Empty)
            return BadRequest("Appointment ID is missing");

        var prescriptions = _context.Prescriptions
            .Where(p => p.AppointmentId == appointmentId)
            .Include(p => p.Appointment)
                .ThenInclude(a => a.Patient)
            .Include(p => p.Appointment)
                .ThenInclude(a => a.Doctor)
            .Select(p => new PrescriptionHistoryVm
            {
                PrescriptionId = p.PrescriptionId,
                CreatedOn = p.CreatedOn,
                Diagnosis = p.Diagnosis,
                Medicines = p.Medicines,
                Notes = p.Notes,

                AppointmentDate = p.Appointment.AppointmentDate,
                Symptoms = p.Appointment.Symptoms,
                PatientName = p.Appointment.Patient.Name,
                DoctorName = p.Appointment.Doctor.Name
            })
            .OrderByDescending(p => p.CreatedOn)
            .AsNoTracking()
            .ToList();

        return View(prescriptions);
    }
    public IActionResult Availability()
    {
        var doctorId = Guid.Parse(HttpContext.Session.GetString("DoctorId"));


        var model = _availabilityService.GetAvailability(doctorId);


        if (!model.Any())
        {
            model = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .Select(d => new DoctorAvailabilityDto { DayOfWeek = d })
            .ToList();
        }


        return View(model);
    }
    [HttpPost]
    public IActionResult Availability(List<DoctorAvailabilityDto> model)
    {
        var doctorId = Guid.Parse(HttpContext.Session.GetString("DoctorId"));
        _availabilityService.SaveAvailability(doctorId, model);


        TempData["Success"] = "Availability updated successfully";
        return RedirectToAction("Availability");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePassword(ChangePasswordDto dto)
    {
        try
        {
            // 🔥 Ignore Profile ModelState
            ModelState.Clear();

            var doctorIdString = HttpContext.Session.GetString("DoctorId");
            if (string.IsNullOrEmpty(doctorIdString))
                return RedirectToAction("Login", "Account");

            if (!TryValidateModel(dto))
                return RedirectToAction("Profile");

            Guid doctorId = Guid.Parse(doctorIdString);

            var result = _doctorService.ChangePassword(doctorId, dto);

            if (!result)
            {
                TempData["Error"] = "Current password is incorrect";
                return RedirectToAction("Profile");
            }

            TempData["Success"] = "Password updated successfully";
            return RedirectToAction("Profile");
        }
        catch
        {
            TempData["Error"] = "Password update failed.";
            return RedirectToAction("Profile");
        }
    }




}
