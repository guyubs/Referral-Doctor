using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Referral_Doctor.Models;

public class IntegratedDoctor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IntegratedDoctorId { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    // 包含来自 Doctor 表的属性
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }

    [ForeignKey("Title")]
    public int TitleId { get; set; }
    public Title Title { get; set; }
    public string TitleName { get; set; }

    [ForeignKey("Specialty")]
    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }
    public string SpecialtyName { get; set; }

    // 导航属性用于关联医生的多个地址
    public ICollection<Address> Addresses { get; set; }

    // 包含来自 Address 表的属性
    public string Street1 { get; set; }
    public string Street2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Tel { get; set; }
    public string Fax { get; set; }

    // 导航属性用于关联医生的多个保险公司
    public ICollection<InsuranceCompanies> InsuranceCompanies { get; set; }

    public string InsuranceCoName { get; set; }
}
