using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.BusinessLogic;
using PatientManagement.Models;

namespace PatientManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientsAsync([FromBody] PatientRequestModel requestModel)
        {
            var patients = await _patientRepository.GetAllPatientsAsync(requestModel);

            if (patients == null || patients.Patients.Count == 0)
            {
                return NotFound("No patients found.");
            }

            Response.Headers.Append("X-Total-Count", patients.Patients.Count.ToString());
            Response.Headers.Append("X-Total-Pages", patients.TotalPages.ToString());

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientByIdAsync([FromRoute] int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            return Ok(patient);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPatientAsync([FromBody] PatientCreateRequest patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isPatientExists = await _patientRepository.IsPatientExistsAsync(patient.Email);

            if (isPatientExists)
                return Conflict($"A patient with email {patient.Email} already exists.");

            var createdPatient = await _patientRepository.AddPatientAsync(patient);
            return Ok(createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatientAsync([FromRoute] int id, [FromBody] PatientUpdateRequest patient)
        {
            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);

            if (existingPatient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            var updatedPatient = await _patientRepository.UpdatePatientAsync(existingPatient, patient);
            return Ok(updatedPatient);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPatientAsync(int id, [FromBody] JsonPatchDocument<PatientModel> patient)
        {
            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);

            if (existingPatient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            var patchedPatient = await _patientRepository.PatchPatientAsync(existingPatient, patient);
            return Ok(patchedPatient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientAsync([FromRoute] int id)
        {
            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);

            if (existingPatient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            await _patientRepository.DeletePatientAsync(existingPatient);
            return Ok($"Patient with Id {id} deleted successfully.");
        }
    }
}
