using PatientManagement.Models;

namespace PatientManagement.BusinessLogic.Implementation
{
    public class Filtering
    {
        public IQueryable<PatientModel> ApplyFiltering(IQueryable<PatientModel> patients, string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return patients;
            }

            term = term.ToLower();
            return patients.Where(p =>
                p.FirstName.ToLower().Contains(term) ||
                p.LastName.ToLower().Contains(term) ||
                p.Email.ToLower().Contains(term) ||
                p.ContactNumber.ToLower().Contains(term));
        }
    }
}
