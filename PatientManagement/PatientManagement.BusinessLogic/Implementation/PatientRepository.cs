using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PatientManagement.BusinessLogic.Implementation;
using PatientManagement.Data;
using PatientManagement.Models;

namespace PatientManagement.BusinessLogic
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;
        private readonly IMapper _mapper;
        private readonly MemoryCaching _memoryCaching;
        private readonly Filtering _filtering;
        private readonly Sorting _sorting;
        private readonly Pagination _pagination;

        public PatientRepository(PatientDbContext context, IMapper mapper, MemoryCaching memoryCaching, Filtering filtering, Sorting sorting, Pagination pagination)
        {
            _context = context;
            _mapper = mapper;
            _memoryCaching = memoryCaching;
            _filtering = filtering;
            _sorting = sorting;
            _pagination = pagination;
        }

        public async Task<PagedPatientResult> GetAllPatientsAsync(PatientRequestModel requestModel)
        {
            var cachedResult = _memoryCaching.GetAllPatientsCache(requestModel);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            IQueryable<PatientModel> patients = _context.Patients.AsNoTracking();

            patients = _filtering.ApplyFiltering(patients, requestModel.Term);

            patients = _sorting.ApplySorting(patients, requestModel.Sort);

            var pagedPatients = await _pagination.ApplyPaginationAsync(patients, requestModel);

            _memoryCaching.SetPatientsCache(requestModel, pagedPatients);

            return pagedPatients;
        }

        public async Task<PatientModel?> GetPatientByIdAsync(int id)
        {
            var patient = await _memoryCaching.GetPatientByIdCacheAsync(id,
               async () => await _context.Patients.FindAsync(id));

            return patient;
        }

        public async Task<PatientModel> AddPatientAsync(PatientCreateRequest createdPatient)
        {
            var patient = _mapper.Map<PatientModel>(createdPatient);

            patient.CreatedDate = DateTime.UtcNow;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<PatientModel> UpdatePatientAsync(PatientModel existingPatient, PatientUpdateRequest updateRequest)
        {
            _mapper.Map(updateRequest, existingPatient);
            existingPatient.UpdatedDate = DateTime.UtcNow;

            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
            return existingPatient;
        }

        public async Task<PatientModel> PatchPatientAsync(PatientModel existingPatient, JsonPatchDocument<PatientModel> patient)
        {
            existingPatient.UpdatedDate = DateTime.UtcNow;

            patient.ApplyTo(existingPatient);

            await _context.SaveChangesAsync();
            return existingPatient;
        }

        public async Task DeletePatientAsync(PatientModel existingPatient)
        {
            _context.Patients.Remove(existingPatient);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsPatientExistsAsync(string? email)
        {
            return await _context.Patients.AnyAsync(p => p.Email == email);
        }
    }
}
