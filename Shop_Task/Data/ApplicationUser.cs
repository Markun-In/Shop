using Microsoft.AspNetCore.Identity;

namespace Shop_Task.Data
{
    public class ApplicationUser : IdentityUser
    {
        public double Balance { get; set; } = 0;
    }
}
