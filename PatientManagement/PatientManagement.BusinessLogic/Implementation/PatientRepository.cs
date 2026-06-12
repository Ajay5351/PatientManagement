using Microsoft.EntityFrameworkCore;
using PatientManagement.Data;
using PatientManagement.Models;

namespace PatientManagement.BusinessLogic
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _context;

        public PatientRepository(PatientContext context)
        {
            _context = context;
        }

        public async Task<PagedPatientResult> GetAllPatients(string? term,string? sort,int page,int limit)
        {
            IQueryable<Patient> patients = _context.Patients;

            // Filtering
            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.Trim().ToLower();

                patients = patients.Where(p =>
                    p.FirstName.ToLower().Contains(term) ||
                    p.LastName.ToLower().Contains(term) ||
                    p.Gender.ToLower().Contains(term) ||
                    p.Email.ToLower().Contains(term) ||
                    p.ContactNumber.Contains(term));
            }

            // Sorting
            patients = sort?.ToLower() switch
            {
                "firstname" => patients.OrderBy(p => p.FirstName),
                "-firstname" => patients.OrderByDescending(p => p.FirstName),

                "lastname" => patients.OrderBy(p => p.LastName),
                "-lastname" => patients.OrderByDescending(p => p.LastName),

                "email" => patients.OrderBy(p => p.Email),
                "-email" => patients.OrderByDescending(p => p.Email),

                "weight" => patients.OrderBy(p => p.Weight),
                "-weight" => patients.OrderByDescending(p => p.Weight),

                "height" => patients.OrderBy(p => p.Height),
                "-height" => patients.OrderByDescending(p => p.Height),

                "createddate" => patients.OrderBy(p => p.CreatedDate),
                "-createddate" => patients.OrderByDescending(p => p.CreatedDate),

                _ => patients.OrderBy(p => p.Id)
            };

            // Pagination
            var totalCount = await patients.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)limit);

            var pagedPatients = await patients
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PagedPatientResult
            {
                Patients = pagedPatients,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<Patient> AddPatients(Patient patient)
        {
            bool emailExists = await _context.Patients
                .AnyAsync(x => x.Email == patient.Email);

            if (emailExists)
                throw new Exception("Email already exists.");

            bool contactExists = await _context.Patients
                .AnyAsync(x => x.ContactNumber == patient.ContactNumber);

            if (contactExists)
                throw new Exception("Contact Number already exists.");

            patient.CreatedDate = DateTime.UtcNow;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
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
