namespace PasswordLookupApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Plaintext for now (demo only!)


          // New fields
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
       
    }
}
