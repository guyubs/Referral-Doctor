using Referral_Doctor.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReferralDoctor.Models
{

    public class DoctorInsurance
    {
        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }

        public int? InsuranceId { get; set; }
        [ForeignKey("InsuranceId")]
        public Insurance? Insurance { get; set; }

        public string? Note { get; set; }

        public bool? IsDeleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? CreatedDateTime { get; set; }

        public DateTimeOffset? ModifiedDateTime { get; set; }

    }

}