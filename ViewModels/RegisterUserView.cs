using System.ComponentModel.DataAnnotations;

namespace Movie_Project.ViewModels;

public class RegisterUserView
{
    public int UserId { get; set; } // Primary Key

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string? Name { get; set; } // Kullanıcının adı

    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters.")]
    public string? Surname { get; set; } // Kullanıcının soyadı

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(25, ErrorMessage = "Username cannot exceed 25 characters.")]
    public string? Username { get; set; } // Kullanıcının kullanıcı adı

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; } // Kullanıcının e-posta adresi

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string? Password { get; set; } // Parola
}
