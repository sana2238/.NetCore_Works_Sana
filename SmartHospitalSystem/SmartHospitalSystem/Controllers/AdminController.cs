using Hospital_Domain.Data;
using Hospital_Domain.Dtos;
using Hospital_Domain.Enum;
using Hospital_Domain.Model;
using Hospital_Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHospitalSystem.Extensions;

namespace SmartHospitalSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDoctorService _d;
        private readonly IEmailService _emailService;


        public AdminController(AppDbContext context, IDoctorService d,IEmailService emailService)
        {
            _context = context;
            _d = d;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            // Try Admin Login
            var admin = _context.AdminDetails.FirstOrDefault(a => a.Email == Email && a.PasswordHash == Password);
            if (admin != null)
            {
                // Store info in session
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("UserEmail", admin.Email);

                return RedirectToAction("Dashboard", "Admin");
            }

            // Try Doctor Login
            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == Email && d.PasswordHash == Password);
            if (doctor != null)
            {
                HttpContext.Session.SetString("UserRole", "Doctor");
                HttpContext.Session.SetString("UserEmail", doctor.Email);
                HttpContext.Session.SetString("DoctorId", doctor.DoctorId.ToString());
                HttpContext.Session.SetString("DoctorName", doctor.Name);

                // Redirect to Doctor Dashboard or profile page
                return RedirectToAction("Dashboard", "Admin"); // or a separate doctor dashboard
            }

            // Try Patient Login (if needed)
            var patient = _context.Patients.FirstOrDefault(p => p.Email == Email && p.PasswordHash == Password);
            if (patient != null)
            {
                HttpContext.Session.SetString("UserRole", "Patient");
                HttpContext.Session.SetString("UserEmail", patient.Email);

                // Redirect to patient page
                return RedirectToAction("Index", "Patient");
            }

            // Invalid login
            ModelState.AddModelError("", "Invalid Credentials");
            return View();
        }

        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [SessionAuthorize("Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllPatients()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }

        [HttpPost]
        public IActionResult AllPatients(string search)
        {
            var patients = _context.Patients
                .Where(p => p.Name.Contains(search) || p.Phone.Contains(search))
                .ToList();

            ViewData["Search"] = search;
            return View(patients);
        }
        [Authorize]
        public IActionResult TestAuth()
        {
            return Content(User.IsInRole("Admin")
                ? "You ARE Admin"
                : "You are NOT Admin");
        }
        [SessionAuthorize("Admin")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return Unauthorized("Admin access only");

            if (id == Guid.Empty)
                return BadRequest("Doctor ID is empty");

            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound("Doctor not found");

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            TempData["Success"] = "Doctor deleted successfully";
            return RedirectToAction("AllDoctors");
        }


        [HttpGet]
        public IActionResult DoctorProfile(Guid id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin" && role != "Doctor")
                return RedirectToAction("Login", "Admin");

            if (id == Guid.Empty)
                return BadRequest("Doctor ID is empty");

            var doctor = _context.Doctors
                .FirstOrDefault(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound("Doctor not found");

            return View(doctor);
        }





        [SessionAuthorize("Admin")]
        public IActionResult AllDoctors()
        {
            var doctors = _context.Doctors.ToList();
            return View(doctors);
        }
        [HttpGet]
        public IActionResult AllAppointments()
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToList();

            return View(appointments);
        }
    }
}
