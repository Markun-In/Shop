using Microsoft.AspNetCore.Identity;

namespace Identity_in_MVC_Task.Data
{
    public class ApplicationUser : IdentityUser
    {
        public double Balance { get; set; } = 0;
    }
}
