using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movie_Project.Models;

namespace Movie_Project.ViewModels
{
    public class MovieInfo
    {
        public int MovieId { get; set; } // Primary Key
        
        [Required]
        [StringLength(200)]
        public string? Title { get; set; } // Filmin adı
        
        public string? Description { get; set; } // Filmin açıklaması
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } // Çıkış tarihi
        
        [Range(1, 600)]
        public int Duration { get; set; } // Süresi (dakika olarak)
        
        public int CategoryId { get; set; } // Türü (Foreign Key)
        
        
        [StringLength(50)]
        public string? Language { get; set; } // Filmin dili
        [Required]
        public IFormFile? PosterUrl { get; set; } // Afiş görselinin URL'si
         // Ortalama kullanıcı puanı
    
    }
}
