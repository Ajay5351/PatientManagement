using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Data;
using PatientManagement.Repository;

namespace PatientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient?>> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
    }
}
