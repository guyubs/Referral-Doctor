using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your username")]
    public string? UserName { get; set; }

    // DataType(DataType.Password)表示这个属性是密码类型，会在前端隐藏输入内容。
    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
