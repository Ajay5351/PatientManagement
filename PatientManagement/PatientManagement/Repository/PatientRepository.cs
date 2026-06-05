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

        public async Task<Patient> AddPatients(Patient patient)
        {
            if (patient == null)
                throw new Exception("Patient data is required.");

            if (string.IsNullOrWhiteSpace(patient.FirstName))
                throw new Exception("First Name is required.");

            if (string.IsNullOrWhiteSpace(patient.LastName))
                throw new Exception("Last Name is required.");

            if (patient.DateOfBirth == default)
                throw new Exception("Date of Birth is required.");

            if (string.IsNullOrWhiteSpace(patient.Gender))
                throw new Exception("Gender is required.");

            if (string.IsNullOrWhiteSpace(patient.ContactNumber))
                throw new Exception("Contact Number is required.");

            if (patient.Weight <= 0)
                throw new Exception("Weight must be greater than 0.");

            if (patient.Height <= 0)
                throw new Exception("Height must be greater than 0.");

            if (string.IsNullOrWhiteSpace(patient.Email))
                throw new Exception("Email is required.");

            if (string.IsNullOrWhiteSpace(patient.Address))
                throw new Exception("Address is required.");

            bool emailExists = await _context.Patients
                .AnyAsync(x => x.Email == patient.Email);

            if (emailExists)
                throw new Exception("Email already exists.");

            bool contactExists = await _context.Patients
                .AnyAsync(x => x.ContactNumber == patient.ContactNumber);

            if (contactExists)
                throw new Exception("Contact Number already exists.");

            patient.CreatedDate = DateTime.UtcNow;
            patient.UpdatedDate = DateTime.UtcNow;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            if (patient == null)
                throw new Exception("Patient data is required.");

            var existingPatient = await _context.Patients.FindAsync(patient.Id);

            if (existingPatient == null)
                throw new Exception("Patient not found.");

            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.ContactNumber = patient.ContactNumber;
            existingPatient.Weight = patient.Weight;
            existingPatient.Height = patient.Height;
            existingPatient.Email = patient.Email;
            existingPatient.Address = patient.Address;
            existingPatient.MedicalComments = patient.MedicalComments;
            existingPatient.AnyMedicationsTaking = patient.AnyMedicationsTaking;
            existingPatient.UpdatedDate = DateTime.UtcNow;

            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
            return existingPatient;
        }

        public async Task DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                throw new Exception("Patient not found.");

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
