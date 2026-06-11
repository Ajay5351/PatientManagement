using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Data;
using PatientManagement.Models;

namespace PatientManagement.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _context;

        public PatientRepository(PatientContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            return await _context.Patients.ToListAsync();
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
