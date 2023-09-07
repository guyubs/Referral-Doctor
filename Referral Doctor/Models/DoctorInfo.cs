using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Referral_Doctor.Models
{
    public class DoctorInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorID { get; set; } // 添加ID属性

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Cell { get; set; }  // ?表示可以为null

        public string TitleName { get; set; }

        public string SpecialtyName { get; set; }

        public string InsuranceCoName { get; set; }

        public string Address { get; set; }

        public string? Note { get; set; }

        public bool? Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
    }
}
