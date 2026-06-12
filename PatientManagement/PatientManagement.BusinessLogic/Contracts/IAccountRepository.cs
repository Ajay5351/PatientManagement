using Microsoft.AspNetCore.Identity;
using PatientManagement.Models;

namespace PatientManagement.BusinessLogic
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignupAsync(SignupModel signupModel);
        Task<string> LoginAsync(SignInModel signInModel);
    }
}
