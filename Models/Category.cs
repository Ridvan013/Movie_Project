using System.ComponentModel.DataAnnotations;

namespace Movie_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Primary Key
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = String.Empty; // Tür adı (ör. Aksiyon, Komedi)
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
