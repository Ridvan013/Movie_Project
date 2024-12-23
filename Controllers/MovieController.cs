using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Project.Models;
using Movie_Project.ViewModels;



 

namespace Movie_Project.Controllers
{
	public class MovieController : Controller
	{
		private readonly MovieDbContext _context;

		public MovieController(MovieDbContext context)
		{
			_context = context;
		}

		// GET: AddMovie
[HttpGet]
[Authorize(Roles = "Admin")]
public IActionResult AddMovie()
{
	ViewBag.Categories = _context.Categories
		.Select(c => new SelectListItem
		{
			Value = c.CategoryId.ToString(),
			Text = c.Name
		})
		.ToList();

	return View();
}

		[HttpPost]
		public async Task<IActionResult> AddMovie(MovieInfo viewModel)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = "Please fill out all fields correctly.";
				return View(viewModel);
			}

			if (viewModel.PosterUrl != null && viewModel.PosterUrl.Length > 0)
			{   
				// Dosyanın adını ve uzantısını al
				var fileName = Path.GetFileName(viewModel.PosterUrl.FileName); // Örn: "image.jpg"
				var fileExtension = Path.GetExtension(viewModel.PosterUrl.FileName); // Örn: ".jpg"
				
				// Dosyayı belirli bir dizine kaydet
				var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
				if (!Directory.Exists(uploadPath))
				{
					Directory.CreateDirectory(uploadPath);
				}

				var filePath = Path.Combine(uploadPath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await viewModel.PosterUrl.CopyToAsync(stream);
				}
				var category = _context.Categories.FirstOrDefault(c=>c.CategoryId == viewModel.CategoryId);

				// ViewModel'den Movie Model'e eşleme
				var movie = new Movie
				{
					Title = viewModel.Title,
					Description = viewModel.Description,
					ReleaseDate = DateTime.SpecifyKind(viewModel.ReleaseDate, DateTimeKind.Utc),
					Duration = viewModel.Duration,
					Category = category,
					Language = viewModel.Language,
					PosterUrl = "/images/" + fileName
				};

			
					// Veritabanına ekleme işlemi
					_context.Movies.Add(movie);
					await _context.SaveChangesAsync();

					// Başarılı ekleme sonrası kullanıcıyı yönlendir
					return RedirectToAction("Index", "Home");
			
			}

			// Eğer dosya yüklenmezse veya koşullar sağlanmazsa buraya dön
			ViewBag.Error = "Please upload a valid poster image.";
			return View(viewModel);
		}

		 public async Task<IActionResult> MovieList(string search, int? categoryId)
		{
			var categories = await _context.Categories.ToListAsync(); // Kategorileri al
			ViewBag.Categories = categories;

			var moviesQuery = _context.Movies
				.Include(m => m.Category)
				.AsQueryable();

			// Film adına göre arama
			if (!string.IsNullOrEmpty(search))
			{
				moviesQuery = moviesQuery.Where(m => m.Title!.Contains(search));
			}

			// Kategoriye göre filtreleme
			if (categoryId.HasValue)
			{
				moviesQuery = moviesQuery.Where(m => m.CategoryId == categoryId.Value);
			}

			var movies = await moviesQuery.ToListAsync();
			return View(movies);
        }

		public async Task<IActionResult> Details(int movieId)
{
	var movie = await _context.Movies
		.Include(m => m.Reviews)
			.ThenInclude(r => r.User) // Kullanıcı bilgilerini de dahil ediyoruz
		.FirstOrDefaultAsync(p => p.MovieId == movieId);

	if (movie == null)
	{
		return NotFound(); // Film bulunamadıysa hata döndür
	}

	return View(movie);
}


	   public IActionResult AddReview(int MovieId, string CommentText)
{
	// Oturumdaki kullanıcının adını al
	var username = User.Identity?.Name;

	if (string.IsNullOrEmpty(username))
	{
		// Kullanıcı oturumu yoksa hata mesajı göster veya giriş sayfasına yönlendir
		return RedirectToAction("Login", "User");
	}

	// Kullanıcıyı veritabanından çek
	var user = _context.Users.FirstOrDefault(u => u.Username == username);

	if (user == null)
	{
		// Kullanıcı veritabanında bulunamadıysa hata mesajı göster
		return BadRequest("User not found.");
	}

	// Yorum verisini oluştur
	var entity = new Review
	{
		Comment = CommentText,
		ReviewDate = DateTime.UtcNow,
		MovieId = MovieId,
		User = user
	};

	// Yorumu veritabanına ekle
	_context.Reviews.Add(entity);
	_context.SaveChanges();

	// Detay sayfasına yönlendir
	return RedirectToAction("Details", "Movie", new { movieId = MovieId });
}


	}
}
