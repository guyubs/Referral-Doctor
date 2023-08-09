using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Referral_Doctor.Models
{

    public class DoctorAddress
    {
        [Key, Column(Order = 1)]
        public int DoctorId { get; set; }

        [Key, Column(Order = 2)]
        public int AddressId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public string? Note { get; set; }

        public bool? Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

    }

}