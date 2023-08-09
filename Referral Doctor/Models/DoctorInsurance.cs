using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Referral_Doctor.Models
{
    public class DoctorInsurance
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("InsuranceId")]
        public Insurance? Insurance { get; set; }

        public int InsuranceId { get; set; }

        public string? Note { get; set; }

        public bool? Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

    }
}
