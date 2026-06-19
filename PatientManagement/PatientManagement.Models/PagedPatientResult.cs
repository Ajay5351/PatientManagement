namespace PatientManagement.Models
{
    public class PagedPatientResult
    {
        public List<PatientModel> Patients { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}