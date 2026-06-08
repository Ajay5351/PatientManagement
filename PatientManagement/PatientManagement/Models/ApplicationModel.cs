using Microsoft.AspNetCore.Identity;

namespace PatientManagement.Models
{
    public class ApplicationModel : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
