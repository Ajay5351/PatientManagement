using PatientManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientManagement.BusinessLogic.Implementation
{
    public class Pagination
    {
        public async Task<PagedPatientResult> ApplyPaginationAsync(IQueryable<PatientModel> patients, PatientRequestModel requestModel)
        {
            var totalCount = await patients.CountAsync();

            var pagedPatients = await patients
                .Skip((requestModel.Page - 1) * requestModel.Limit)
                .Take(requestModel.Limit)
                .ToListAsync();

            return new PagedPatientResult
            {
                Patients = pagedPatients,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / requestModel.Limit)
            };
        }
    }
}
