// 没有使用双联合主键DoctorId，AddressId的原因是在Edit该表时若修改了其中一个联合主键，会检索不到该行信息。
// 也可以通过不允许修改联合主键来避免上面的问题。

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Referral_Doctor.Models
{
    public class DoctorAddress
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public int AddressId { get; set; }

        public string? Note { get; set; }

        public bool? Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

    }
}
