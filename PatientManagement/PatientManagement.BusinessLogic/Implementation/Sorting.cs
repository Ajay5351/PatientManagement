using PatientManagement.Models;

namespace PatientManagement.BusinessLogic.Implementation
{
    public class Sorting
    {
        public IQueryable<PatientModel> ApplySorting(IQueryable<PatientModel> patients, string? sort)
        {
            return sort?.ToLower() switch
            {
                "firstname" => patients.OrderBy(p => p.FirstName),
                "-firstname" => patients.OrderByDescending(p => p.FirstName),

                "lastname" => patients.OrderBy(p => p.LastName),
                "-lastname" => patients.OrderByDescending(p => p.LastName),

                "email" => patients.OrderBy(p => p.Email),
                "-email" => patients.OrderByDescending(p => p.Email),

                "dateofbirth" => patients.OrderBy(p => p.DateOfBirth),
                "-dateofbirth" => patients.OrderByDescending(p => p.DateOfBirth),

                _ => patients.OrderBy(p => p.Id)
            };
        }
    }
}
