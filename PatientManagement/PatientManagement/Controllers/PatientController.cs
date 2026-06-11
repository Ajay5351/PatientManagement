using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Models;
using PatientManagement.Repository;

namespace PatientManagement.Controllers
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

        [HttpGet("")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _patientRepository.GetAllPatients();

            if (patients == null || !patients.Any())
            {
                return NotFound("No patients found.");
            }

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int id)
        {
            var patient = await _patientRepository.GetPatientById(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdPatient = await _patientRepository.AddPatients(patient);

                return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.Id }, createdPatient);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while checking for existing patient: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] int id, [FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            patient.Id = id;

            try
            {
                var updatedPatient = await _patientRepository.UpdatePatient(patient);

                return Ok(updatedPatient);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the patient: {ex.Message}");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient([FromRoute] int id)
        {
            try
            {
                await _patientRepository.DeletePatient(id);
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the patient: {ex.Message}");
            }
        }
    }
}
