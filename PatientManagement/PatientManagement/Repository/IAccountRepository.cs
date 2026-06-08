using Microsoft.AspNetCore.Identity;
using PatientManagement.Models;

namespace PatientManagement.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignupAsync(SignupModel signupModel);
    }
}
