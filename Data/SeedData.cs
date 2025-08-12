using Microsoft.EntityFrameworkCore;
using PasswordLookupApp.Models;

namespace PasswordLookupApp.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "Asad", Password = "1234" },
                    new User { Username = "JohnDoe", Password = "abcd" }
                );
                context.SaveChanges();
            }
        }
    }
}
