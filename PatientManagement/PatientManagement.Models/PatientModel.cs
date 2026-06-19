using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagement.Models
{
    public class PatientModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        [Column(TypeName = "nvarchar(10)")]
        public string Gender { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string? ContactNumber { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Address { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? MedicalComments { get; set; }

        public bool? AnyMedicationsTaking { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
