using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Models
{
    public class PatientRequestModel
    {
        [StringLength(50, ErrorMessage = "Search term cannot exceed 50 characters.")]
        public string? Term { get; set; }

        [RegularExpression("^(-?firstname|-?lastname|-?email|-?dateofbirth)?$",
        ErrorMessage = "Sort must be firstname, -firstname, lastname, -lastname, email, -email, dateofbirth, or -dateofbirth.")]
        public string? Sort { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;

        [Range(1, 100,
            ErrorMessage = "Limit must be between 1 and 100.")]
        public int Limit { get; set; } = 5;
    }
}
