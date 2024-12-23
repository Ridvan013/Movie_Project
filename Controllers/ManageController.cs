using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Project.Models;
using Movie_Project.ViewModels;

namespace Movie_Project.Controllers
{
	public class ManageController : Controller
	{
		private readonly MovieDbContext _context;

		public ManageController(MovieDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> DeleteMovie(int movieId){
			var willRemoveUser = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);

			if (willRemoveUser == null)
			{
				throw new Exception("There is no user in the system with id :" + movieId);
			}

			_context.Movies.Remove(willRemoveUser);
			await _context.SaveChangesAsync();

			return RedirectToAction("MovieList","Movie");
		}
[HttpGet]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> UpdateMovie(int movieId)
{
	// Kategorileri ViewBag'e yükle
	ViewBag.Categories = _context.Categories
		.Select(c => new SelectListItem
		{
			Value = c.CategoryId.ToString(),
			Text = c.Name
		})
		.ToList();

	// Güncellenecek filmi veritabanından getir
	var willUpdateMovie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);

	// Eğer film bulunamazsa hata döndür
	if (willUpdateMovie == null)
	{
		return NotFound("Movie not found.");
	}

	// Film bilgilerini ViewModel'e dönüştür
	var willUpdateMovieView = new MovieInfo
	{
		MovieId = willUpdateMovie.MovieId,
		Title = willUpdateMovie.Title,
		Description = willUpdateMovie.Description,
		ReleaseDate = willUpdateMovie.ReleaseDate,
		Duration = willUpdateMovie.Duration,
		CategoryId = willUpdateMovie.CategoryId,
		Language = willUpdateMovie.Language
	};

	// Görünümü döndür
	return View(willUpdateMovieView);
}


[HttpPost]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> UpdateMovie(MovieInfo updatedMovie)
{
	
	 ViewBag.Categories = _context.Categories
		.Select(c => new SelectListItem
		{
			Value = c.CategoryId.ToString(),
			Text = c.Name
		})
		.ToList();
	
	if (ModelState.IsValid) // Form verilerinin doğruluğunu kontrol eder
	{
		var fileName = Path.GetFileName(updatedMovie.PosterUrl!.FileName); // Örn: "image.jpg"
				var fileExtension = Path.GetExtension(updatedMovie.PosterUrl.FileName); // Örn: ".jpg"
				
				// Dosyayı belirli bir dizine kaydet
				var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
				if (!Directory.Exists(uploadPath))
				{
					Directory.CreateDirectory(uploadPath);
				}

				var filePath = Path.Combine(uploadPath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await updatedMovie.PosterUrl.CopyToAsync(stream);
				}
				var category = _context.Categories.FirstOrDefault(c=>c.CategoryId == updatedMovie.CategoryId);
		var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == updatedMovie.MovieId);
		if (existingMovie != null)
		{
			// Mevcut film bilgilerini güncelle
			existingMovie.Title = updatedMovie.Title;
			existingMovie.Description = updatedMovie.Description;
			existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
			existingMovie.Duration = updatedMovie.Duration;
			existingMovie.Category = category;
			existingMovie.PosterUrl =  "/images/" + fileName;
			existingMovie.Language = updatedMovie.Language;

			// Değişiklikleri veritabanına kaydet
			await _context.SaveChangesAsync();

			// Güncelleme sonrası başka bir sayfaya yönlendirme (örneğin liste sayfası)
			return RedirectToAction("MovieList","Movie");
		}

		// Eğer film bulunamazsa hata döner
		ModelState.AddModelError("", "Movie not found.");
	}

	// Eğer model doğrulama başarısızsa veya film bulunamazsa aynı formu tekrar göster
	return View (updatedMovie);

}

	[Authorize(Roles = "Admin")]
	public IActionResult AllUsers(){
		var userList = _context.Users;

		var UsersView = userList.Select(user=>new UserView{
			UserId = user.UserId,
			Name = user.Name,
			Surname = user.Surname,
			UserName = user.Username,
			Email = user.Email
		});

		return View(UsersView);

		
	}

		
	}
}
