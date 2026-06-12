using PatientManagement.Models;

namespace PatientManagement.BusinessLogic
{
    public interface IPatientRepository
    {
        Task<PagedPatientResult> GetAllPatients(string? term, string? sort, int page, int limit);
        Task<Patient?> GetPatientById(int id);
        Task<Patient> AddPatients(Patient patient);
        Task<Patient> UpdatePatient(Patient patient);
        Task DeletePatient(int id);
    }
}
