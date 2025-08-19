using Microsoft.AspNetCore.Identity;

namespace OrderBooking.Services.AuthAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
