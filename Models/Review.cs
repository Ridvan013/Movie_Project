using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Project.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; } // Primary Key
        
        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; } // Foreign Key - Film Kimliği
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; } // Foreign Key - Kullanıcı Kimliği
        
        [Required]
        [Range(1, 10)]
        public int Rating { get; set; } // Kullanıcının verdiği puan (1-10 arasında)
        
        [StringLength(1000)]
        public string? Comment { get; set; } // Kullanıcının yorumu
        
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow; // Yorumun yapıldığı tarih (varsayılan: şu anki tarih)
        
        // Navigation Properties
        public Movie? Movie { get; set; }
        public User? User { get; set; }
    }
}
