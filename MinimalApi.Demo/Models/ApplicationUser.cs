using Microsoft.AspNetCore.Identity;

namespace MinimalApi.Demo.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
