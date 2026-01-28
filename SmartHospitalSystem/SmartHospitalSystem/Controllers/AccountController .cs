using Hospital_Domain.Dtos;
using Hospital_Domain.Email;
using Hospital_Domain.Model;
using Hospital_Domain.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartHospitalSystem.ViewModels;
using System.Security.Claims;
namespace SmartHospitalSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly IUserService _service;
        private readonly IEmailService _EmailService;

        public AccountController(IUserService service, IWebHostEnvironment env, IEmailService emailService)
        {
            _service = service;
            _env = env;
            _EmailService = emailService;
        }

        public IActionResult DoctorRegister() => View();


        [HttpPost]
        public IActionResult DoctorRegister(RegisterDoctorViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // ✅ Email already exists check
            if (_service.EmailExists(vm.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(vm);
            }

            string imagePath = "uploads/doctors/default.png";

            if (vm.ProfileImage != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads/doctors");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ProfileImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                vm.ProfileImage.CopyTo(stream);

                imagePath = "uploads/doctors/" + fileName;
            }

            // ⚠️ Store original password for email
            string plainPassword = vm.Password;

            var dto = new RegisterDoctorDto
            {
                Name = vm.Name,
                Email = vm.Email,
                Password = vm.Password,
                Specialization = vm.Specialization,
                PhoneNumber = vm.PhoneNumber,
                Qualification = vm.Qualification,
                ExperienceYears = vm.ExperienceYears,
                AvailableFromDate = vm.AvailableFromDate,
                AvailableToDate = vm.AvailableToDate,
                AvailableFromTime = vm.AvailableFromTime,
                AvailableToTime = vm.AvailableToTime
            };

            var doctor = _service.RegisterDoctor(dto, imagePath);

            // ✅ SEND EMAIL WITH LOGIN DETAILS
            var mailRequest = new MailRequest
            {
                ToEmail = doctor.Email,
                Subject = "Doctor Account Created – SmartHospitalSystem",
                Body = $@"
            <h3>Hi Dr. {doctor.Name},</h3>
            <p>Your doctor account has been created successfully.</p>

            <p><b>Login Details:</b></p>
            <p>Email: <b>{doctor.Email}</b></p>
            <p>Password: <b>{plainPassword}</b></p>

            <p>Please login and change your password immediately.</p>
            <br/>
            <p>Regards,<br/>SmartHospitalSystem Team</p>
        "
            };

            _EmailService.SendEmail(mailRequest);

            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Login() => View();



        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var doctor = _service.AuthenticateDoctor(dto);
            if (doctor == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View(dto);
            }

            // ✅ SESSION
            HttpContext.Session.SetString("DoctorId", doctor.DoctorId.ToString());
            HttpContext.Session.SetString("DoctorName", doctor.Name);
            HttpContext.Session.SetString("DoctorEmail", doctor.Email);
            HttpContext.Session.SetString("UserRole", "Doctor");

            // optional email
            _EmailService.SendEmail(new MailRequest
            {
                ToEmail = doctor.Email,
                Subject = "Login Notification – SmartHospitalSystem",
                Body = $"Hi Dr. {doctor.Name}, you logged in at {DateTime.Now}"
            });

            return RedirectToAction("Dashboard", "Doctor");
        }


    }
}
