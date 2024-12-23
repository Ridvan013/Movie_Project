using System.ComponentModel.DataAnnotations;

namespace Movie_Project.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // Primary Key
        public string? Name { get; set; }
        public string? Surname { get; set; }
        
        [Required]
        [StringLength(100)]
        public string? Username { get; set; } // Kullanıcının kullanıcı adı
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; } // Kullanıcının e-posta adresi
        
        [Required]
        [StringLength(255)]
        public string? Password { get; set; } // Kullanıcının hashlenmiş şifresi
        
        [Required]
        [StringLength(20)]
        public int RoleId{get;set;}
        public Role Role{get;set;} =new Role();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
