﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UserRole
{
    Normal,
    Admin,
    Developer
}

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "请选择用户级别")]
    public UserRole Role { get; set; } = UserRole.Normal;  // 用户级别， 默认Normal

    [Required(ErrorMessage = "请输入用户名")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "用户名长度必须在2到50个字符之间")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "请输入密码")]
    [DataType(DataType.Password)]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "密码长度必须在1到20个字符之间")]
    public string? Password { get; set; }

    public string? PasswordSalt { get; set; }

    [Required(ErrorMessage = "Please Put In Your First Name")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Please Put In Your Last Name")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "请输入出生日期: yyyy-MM-dd")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "请输入电子邮件")]
    [EmailAddress(ErrorMessage = "请输入有效的电子邮件地址")]
    [RegularExpression(@"^(?!.*\.com\.com$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "请输入有效的电子邮件地址")] // 检测.com.com的情况
    public string? PrimaryEmail { get; set; }

    [EmailAddress(ErrorMessage = "请输入有效的电子邮件地址")]
    [RegularExpression(@"^(?!.*\.com\.com$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "请输入有效的电子邮件地址")]
    
    public string? SecondaryEmail { get; set; }

    // [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "请输入有效的手机号")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "请输入Note")]
    [NoteValidation(ErrorMessage = "Note内填入邀请码，请咨询管理员")]
    public string? Note { get; set; }

    public bool? Deleted { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}

// Note可以作为邀请码，来实现不是所有人都能注册。只有获得正确Note的人才能注册。
public class NoteValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        string note = (string)value;
        return note == "123456";
    }
}