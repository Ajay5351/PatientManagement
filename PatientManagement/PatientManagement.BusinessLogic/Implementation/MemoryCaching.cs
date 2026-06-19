using PatientManagement.Models;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace PatientManagement.BusinessLogic.Implementation
{
    public class MemoryCaching
    {
        private readonly ICacheProvider _cacheProvider;

        private readonly MemoryCacheEntryOptions _cacheOptions =
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

        public MemoryCaching(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public PagedPatientResult? GetAllPatientsCache(PatientRequestModel request)
        {
            string cacheKey = $"Patient_{request.Term}_{request.Sort}_{request.Page}_{request.Limit}";

            _cacheProvider.TryGetValue(cacheKey, out PagedPatientResult? result);
            return result;
        }

        public void SetPatientsCache(PatientRequestModel request, PagedPatientResult result)
        {
            string cacheKey = $"Patient_{request.Term}_{request.Sort}_{request.Page}_{request.Limit}";

            _cacheProvider.Set(cacheKey, result, _cacheOptions);
        }

        public async Task<PatientModel?> GetPatientByIdCacheAsync(int id, Func<Task<PatientModel?>> getPatient)
        {
            string cacheKey = $"Patient_{id}";

            if (_cacheProvider.TryGetValue(cacheKey, out PatientModel? patient))
            {
                return patient;
            }

            patient = await getPatient();

            if (patient != null)
            {
                _cacheProvider.Set(cacheKey, patient, _cacheOptions);
            }
            return patient;
        }

        public void RemovePatientCache(int id)
        {
            _cacheProvider.Remove($"Patient_{id}");
        }
    }
}