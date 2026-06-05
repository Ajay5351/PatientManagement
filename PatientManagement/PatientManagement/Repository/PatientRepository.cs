using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Data;

namespace PatientManagement.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _context;
        private readonly IMapper _mapper;

        public PatientRepository(PatientContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return _mapper.Map<List<Patient>>(patients);
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return _mapper.Map<Patient?>(patient);
        }
    }
}
