using Microsoft.AspNetCore.JsonPatch;
using PatientManagement.Models;

namespace PatientManagement.BusinessLogic
{
    public interface IPatientRepository
    {
        Task<PagedPatientResult> GetAllPatientsAsync(PatientRequestModel requestModel);
        Task<PatientModel?> GetPatientByIdAsync(int id);
        Task<PatientModel> AddPatientAsync(PatientCreateRequest createdPatient);
        Task<PatientModel> UpdatePatientAsync(PatientModel existingPatient, PatientUpdateRequest updateRequest);
        Task<PatientModel> PatchPatientAsync(PatientModel existingPatient, JsonPatchDocument<PatientModel> patient);
        Task DeletePatientAsync(PatientModel existingPatient);
        Task<bool> IsPatientExistsAsync(string? email);
    }
}