using Hospital_Domain.Dtos;
using Hospital_Domain.Email;
using Hospital_Domain.Enum;
using Hospital_Domain.Model;
using Hospital_Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartHospitalSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IEmailService _emailService;

        public PatientController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IEmailService emailService) // Inject EmailService
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _emailService = emailService;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(PatientRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            if (_patientService.EmailExists(dto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(dto);
            }

            _patientService.RegisterPatient(dto);

            // Send welcome email
            var mailRequest = new MailRequest
            {
                ToEmail = dto.Email,
                Subject = "Welcome to SmartHospitalSystem",
                Body = $"<h3>Hi {dto.Name},</h3><p>Thank you for registering at SmartHospitalSystem.</p>"
            };
            _emailService.SendEmail(mailRequest);

            return RedirectToAction("Login", "Patient");
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var patient = _patientService.Login(dto);

            if (patient == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(dto);
            }

            HttpContext.Session.SetString("PatientEmail", patient.Email);
            HttpContext.Session.SetString("PatientName", patient.Name);

            // Send login notification email
            var mailRequest = new MailRequest
            {
                ToEmail = patient.Email,
                Subject = "Login Notification",
                Body = $"<h3>Hi {patient.Name},</h3><p>You have successfully logged in at {DateTime.Now}.</p>"
            };
            _emailService.SendEmail(mailRequest);

            return RedirectToAction("Profile", "Patient");
        }

        // GET: Profile
        [HttpGet]
        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("PatientEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Patient");

            var patient = _patientService.GetPatientByEmail(email);
            return View(patient);
        }

        [HttpGet]
        public IActionResult BookAppointment()
        {
            ViewBag.Specializations = _doctorService.GetSpecializations();
            return View(new AppointmentBookingDto());  //Sends an empty AppointmentBookingDto model
        }

        [HttpPost]
        public IActionResult SelectSpecialization(AppointmentBookingDto dto)
        {
            ViewBag.Specializations = _doctorService.GetSpecializations();
            ViewBag.Doctors = _doctorService.GetDoctors(dto.Specialization);
            return View("BookAppointment", dto);  //Re-renders the same page
        }

        [HttpPost]
        public IActionResult SelectDoctor(AppointmentBookingDto dto)
        {
            ViewBag.Specializations = _doctorService.GetSpecializations();
            ViewBag.Doctors = _doctorService.GetDoctors(dto.Specialization);

            var doctor = _doctorService.GetDoctor(dto.DoctorId);

            dto.AvailableFromDate = doctor.AvailableFromDate;
            dto.AvailableToDate = doctor.AvailableToDate;
            dto.AvailableFromTime = doctor.AvailableFromTime;
            dto.AvailableToTime = doctor.AvailableToTime;

            return View("BookAppointment", dto);
        }

        // ============================
        // LOAD AVAILABLE SLOTS
        // ============================
        [HttpPost]
        public IActionResult LoadSlots(AppointmentBookingDto dto)
        {
            ViewBag.Specializations = _doctorService.GetSpecializations();
            ViewBag.Doctors = _doctorService.GetDoctors(dto.Specialization);

            var doctor = _doctorService.GetDoctor(dto.DoctorId);

            // Existing appointments on selected date
            var bookedSlots = _appointmentService
                .GetDoctorAppointments(dto.DoctorId)
                .Where(a => a.AppointmentDate.Date == dto.AppointmentDate.Date)
                .Select(a => a.AppointmentDate)
                .ToList();

            var slots = new List<DateTime>();

            var start = dto.AppointmentDate.Date
                .Add(doctor.AvailableFromTime.Value.ToTimeSpan());

            var end = dto.AppointmentDate.Date
                .Add(doctor.AvailableToTime.Value.ToTimeSpan());

            while (start.AddMinutes(15) <= end)
            {
                if (!bookedSlots.Contains(start))
                    slots.Add(start);

                start = start.AddMinutes(15);
            }

            ViewBag.AvailableSlots = slots;

            return View("BookAppointment", dto);
        }

        // ============================
        // CONFIRM APPOINTMENT
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmAppointment(AppointmentBookingDto dto, DateTime SelectedSlot)
        {
            var email = HttpContext.Session.GetString("PatientEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Patient");

            var patient = _patientService.GetPatientByEmail(email);
            dto.AppointmentDate = SelectedSlot;

            _appointmentService.GetAppointment(dto, patient.PatientId);

            var doctor = _doctorService.GetDoctor(dto.DoctorId);

            _emailService.SendEmail(new MailRequest
            {
                ToEmail = patient.Email,
                Subject = "Your Appointment Confirmation",
                Body = $@"
                <h3>Your Appointment Confirmed</h3>
                <p><b>Doctor:</b> Dr. {doctor.Name}</p>
                <p><b>Date & Time:</b> {SelectedSlot:dd MMM yyyy hh:mm tt}</p>
                <p>Thankyou for choosing <strong>Smart Hospital</strong></p>"
                
            });

            TempData["Success"] = "Appointment booked successfully!";
            return RedirectToAction("Profile");
        }
    
        // GET: View Appointments
        [HttpGet]
        public IActionResult ViewAppointment()
        {
            var email = HttpContext.Session.GetString("PatientEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Patient");

            var patient = _patientService.GetPatientByEmail(email);
            var appointments = _appointmentService.GetAppointmentsByPatient(patient.PatientId);

            return View(appointments);
        }

        // GET: Edit Profile
        [HttpGet]
        public IActionResult EditProfile()
        {
            var email = HttpContext.Session.GetString("PatientEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Patient");

            var patient = _patientService.GetPatientByEmail(email);
            var dto = new PatientRegisterDto
            {
                Name = patient.Name,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address,
                Gender = patient.Gender,
                Age = patient.Age,
                BloodGroup = patient.BloodGroup,
                Place = patient.Place
            };

            return View(dto);
        }

        // POST: Edit Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(PatientRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var email = HttpContext.Session.GetString("PatientEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Patient");

            var patient = _patientService.GetPatientByEmail(email);
            _patientService.UpdatePatient(patient.PatientId, dto);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile", "Patient");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
