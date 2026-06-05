using PatientManagement.Data;

namespace PatientManagement.Repository
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllPatients();
        Task<Patient?> GetPatientById(int id);
    }
}
