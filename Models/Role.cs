using System.ComponentModel.DataAnnotations;

namespace Movie_Project.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; } // Primary Key
        
        [Required]
        [StringLength(50)]
        public string? RoleName { get; set; } // Rol adı (ör. Admin, Viewer)
        public List<User> Users{get;set;} = new List<User>();
    }
}
