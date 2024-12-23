using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Project.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; } // Primary Key
        
        [Required]
        [StringLength(200)]
        public string? Title { get; set; } // Filmin adı
        
        public string? Description { get; set; } // Filmin açıklaması
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } // Çıkış tarihi
        
        [Range(1, 600)]
        public int Duration { get; set; } // Süresi (dakika olarak)
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; } // Türü (Foreign Key)
        
        [ForeignKey("Director")]
        
        [StringLength(50)]
        public string? Language { get; set; } // Filmin dili
        
        [Url]
        public string? PosterUrl { get; set; } // Afiş görselinin URL'si
        
        [Range(0.0, 10.0)]
        public double? Rating { get; set; } // Ortalama kullanıcı puanı
        
        // Navigation Properties
        public Category? Category { get; set; } 
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
