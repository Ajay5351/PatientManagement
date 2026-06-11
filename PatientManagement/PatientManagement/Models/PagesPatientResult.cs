namespace PatientManagement.Models
{
    public class PagedPatientResult
    {
        public List<Patient> Patients { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}