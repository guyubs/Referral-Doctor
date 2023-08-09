using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReferralDoctor.Models
{

    public class Insurance
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsuranceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? InsuranceName { get; set; }

        public string? Note { get; set; }

        public bool? Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? CreatedDateTime { get; set; }

        public DateTimeOffset? ModifiedDateTime { get; set; }

    }

}