using System.ComponentModel.DataAnnotations;

namespace DocTask.Core.Dtos.Authentications;

public class LoginRequestDto
{
    [Required (AllowEmptyStrings = false, ErrorMessage = "User Name is required")]
    public string Username { get; set; } = string.Empty;
    
    [Required (AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [Length(6, 100, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password { get; set; } = string.Empty;
}