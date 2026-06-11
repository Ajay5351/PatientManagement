using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PatientManagement.Caching;
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
        private readonly ICacheProvider _cacheProvider;

        public PatientController(IPatientRepository patientRepository, ICacheProvider cacheProvider)
        {
            _patientRepository = patientRepository;
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? term, [FromQuery] string? sort, [FromQuery] int page = 1,
        [FromQuery] int limit = 5)
        {
            string cacheKey = $"Patients_{term}_{sort}_{page}_{limit}";

            if (!_cacheProvider.TryGetValue(cacheKey,
                out PagedPatientResult? result))
            {
                result = await _patientRepository.GetAllPatients(term, sort, page, limit);

                if (result == null)
                    return NotFound("No patients found");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(30),

                    SlidingExpiration =
                        TimeSpan.FromSeconds(30),

                    Size = 1000
                };

                _cacheProvider.Set(
                    cacheKey,
                    result,
                    cacheEntryOptions);
            }

            Response.Headers.Append(
                "X-Total-Count",
                result!.TotalCount.ToString());

            Response.Headers.Append(
                "X-Total-Pages",
                result.TotalPages.ToString());

            return Ok(result.Patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int id)
        {
            if (!_cacheProvider.TryGetValue(CacheKeys.Patient, out Patient? patient))
            {
                patient = await _patientRepository.GetPatientById(id);

                if (patient == null)
                    return NotFound("Patient not found");

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1000
                };
                _cacheProvider.Set(CacheKeys.Patient, patient, cacheEntryOptions);
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
